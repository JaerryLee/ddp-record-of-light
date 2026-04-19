# 07 · Technical Design

> 시스템 구조·퍼포먼스 예산·빌드 파이프라인. 메커닉 동작 사양은 [01](01-game-design.md), 에셋 예산은 [04 · Art §7](04-art-direction.md).

## 1. Stack

| 항목 | 값 | 근거 |
|---|---|---|
| 엔진 | **Unity 2022.3 LTS** | 안정 LTS, 본선까지 업데이트 불필요 |
| 렌더 파이프 | **Universal Render Pipeline (URP) 14.x** | 퍼포먼스 + 모바일 가능성(본선) |
| 언어 | C# (.NET Standard 2.1) | Unity 표준 |
| 입력 | **New Input System 1.7+** | 키맵·게임패드 확장 용이 |
| UI | UGUI (1차) → UIToolkit (본선) | 1차는 검증된 UGUI |
| 오디오 | Unity AudioMixer + 커스텀 Snapshot 스위칭 | 내장 충분 |
| 세이브 | JSON via `System.Text.Json` on Application.persistentDataPath | 단일 슬롯이면 충분 |
| VCS | Git + Git LFS | 본 리포 |
| CI | GitHub Actions (Unity builds via unity-builder) | 본선 확장 |

## 2. Target Spec

### 2.1 최소 사양
| 구성 | 값 |
|---|---|
| OS | Windows 10 64-bit (Build 19041+) |
| CPU | Intel i5-6600 / AMD Ryzen 5 1400 |
| GPU | NVIDIA GTX 1060 6GB / AMD RX 580 |
| RAM | 8 GB |
| Storage | 4 GB (빌드+에셋), 2 GB 자유 공간 |

### 2.2 권장 사양
- GPU GTX 1660 / RTX 3050 이상, RAM 16 GB

### 2.3 해상도·프레임 목표
- **1920×1080 @ 60fps** (권장 사양 기준, Mid 그래픽)
- 1440p 지원(권장 사양), 2160p는 본선 이후
- Frame pacing: ≤ 2 ms variance

## 3. Performance Budget

### 3.1 Frame Budget @ 60fps (16.6 ms)

| 영역 | 예산 (ms) |
|---|---|
| CPU Main (Scripts + Update) | 4.0 |
| CPU Render (SRP Batcher) | 2.0 |
| Physics | 0.5 |
| Animation (Hologram only) | 0.5 |
| Audio | 0.5 |
| GPU Render (URP Opaque+Transparent) | 6.0 |
| Post | 1.5 |
| 버퍼 | 1.6 |

### 3.2 씬별 Draw Call 상한
- Zone 1 · 2 · 3 각각 프러스텀 내 900 이하
- 홀로그램 씬 순간 +50 허용

### 3.3 메모리
- 텍스처 풀 ≤ 1.2 GB VRAM (Mid tier)
- 오디오 스트리밍 가능한 BGM만 로드, SFX는 Decompress on Load

## 4. Project Architecture

### 4.1 레이어 분리

```
 +-----------------------------------------------+
 |  Presentation        (UI · VFX · Subtitles)   |
 +-----------------------------------------------+
 |  Gameplay            (Prism · Beam · Crystal) |
 +-----------------------------------------------+
 |  Systems             (Save · Audio · Scene)   |
 +-----------------------------------------------+
 |  Core                (Services · Events)      |
 +-----------------------------------------------+
```

### 4.2 주요 클래스 (Scripts/Gameplay)

| 클래스 | 책임 | 의존 |
|---|---|---|
| `PlayerController` | 1인칭 이동·카메라·상호작용 레이캐스트 | New Input |
| `InteractionTarget` (interface) | Node/Crystal/Sign 공통 인터페이스 | - |
| `PrismNode` | 배치 가능한 슬롯. 현재 프리즘 reference | - |
| `PrismInstance` | 배치된 프리즘. Type·Rotation·Color mask | - |
| `BeamEmitter` | 고정 광원. 초기 빔 스폰 | - |
| `BeamSegment` | LineRenderer + 물리 레이캐스트 | - |
| `BeamResolver` | 한 프레임당 모든 빔 경로 재계산 | - |
| `MemoryCrystal` | 색·프리즘 조건 평가, Vignette 트리거 | `VignettePlayer` |
| `VignettePlayer` | 홀로그램 + 보이스 + 자막 재생 | `AudioService`, `SubtitleService` |
| `ZoneController` | 씬 진입 이벤트, 체크포인트 | `SaveService` |

### 4.3 주요 서비스 (Scripts/Systems)

| 서비스 | 책임 |
|---|---|
| `AudioService` | AudioMixer 스냅샷, 볼륨 슬라이더 연동 |
| `SubtitleService` | JSON 스크립트 로드, 순차 표시 |
| `SaveService` | 진행 슬롯 직렬화/역직렬화 |
| `InputService` | Input Action asset 래퍼, 리바인딩 저장 |
| `SceneFlowService` | Boot → Menu → Zones 전환, Addressables 또는 SceneManager 기반 |
| `ArchiveAIService` | AI 상태(`Idle`·`Hinting`·`Speaking`) 기반 라인 셀렉션 |

### 4.4 의존성 관리
- **DI 프레임워크 사용 안 함** (과한 인프라). ScriptableObject 싱글톤(`Service Locator`) 수준 유지
- 예: `AudioService.Instance.Play("SFX_Prism_Place_A")`

## 5. Beam Resolver 알고리즘

```
매 프레임 (FixedUpdate는 피하고 LateUpdate):
  for each BeamEmitter:
    path = [emitter.position]
    dir = emitter.forward
    color = emitter.color
    hits = 0
    while hits < MAX_REFLECTIONS (3):
      raycast(dir, MAX_RANGE=40m)
      if hit Prism:
         reflect or split per Prism.type
         path += hit.point
         update dir (and color for Splitter)
         hits++
         continue
      if hit Crystal:
         path += hit.point
         schedule Crystal.MaybeActivate(color)
         break
      if hit World:
         path += hit.point
         break
    beam.lineRenderer.SetPositions(path)
```

### 5.1 최적화
- Prism·Crystal은 `Physics Layer`에 분리 → 레이캐스트 필터링
- LineRenderer는 한 Emitter당 **Splitter 분기 후 최대 3개**
- 변화 없는 프레임은 `_isDirty = false` 스킵

### 5.2 Color Mask
- `enum LightColor { White = 0b111, R = 0b100, G = 0b010, B = 0b001, RG=..., None = 0 }`
- Crystal.requiredColor == beam.color 일치 시 활성화

## 6. Scene Structure

```
Scenes/
├── Boot                     — 전처리(서비스 초기화 + SceneFlow 트리거)
├── MainMenu                 — 타이틀
└── Zones/
    ├── Zone1_Alimter
    ├── Transition1
    ├── Zone2_Baeumter
    ├── Transition2
    └── Zone3_Salimter       — 엔딩 시퀀스 포함
```

- **Additive Loading 사용 안 함** (1차 스코프). 단일 씬 전환
- 본선 확장에서 Addressables 도입 고려

## 7. Save System

### 7.1 Save Data 구조

```csharp
[Serializable]
public class SaveData {
    public string version;              // "1.0"
    public string currentScene;         // "Zone2_Baeumter"
    public HashSet<string> unlockedCrystals; // ["C1","C2","C3"]
    public PlayerPrefsSnapshot prefs;   // 볼륨·자막·FOV
    public DateTime lastPlayed;
}
```

### 7.2 저장 타이밍
- 씬 전환 직후
- 크리스털 활성화 후
- Pause → Quit 시

### 7.3 위치
- `%AppData%/../LocalLow/<Company>/<Product>/save.json`
- 수동 백업 가능 (권장하지 않음)

## 8. Rendering

### 8.1 URP Asset
- Shadow Distance: 30 m
- Shadow Cascades: 1 (근거리 씬)
- Opaque Texture: ON (홀로그램 리프랙션 활용)
- Depth Texture: ON

### 8.2 Post Processing (Volume Profiles)

공통:
- ACES Tonemap
- Vignette intensity 0.25, smoothness 0.5
- Bloom threshold 0.9, intensity 0.6
- Color Adjustments: Zone별 LUT 오버라이드

Zone 2 Volume:
- Chromatic Aberration intensity 0.1 (디지털 글리치 암시)

Zone 3 Volume:
- White Balance +8 (따뜻함)
- Grain off

### 8.3 Lighting
- Mixed Lighting + Shadowmask
- Light Probe Group 씬당 1개 이상
- Reflection Probe: Box 1개 (Zone 2 실내)

## 9. Quality Tiers

| Tier | Renderer | Shadow Res | SSAO | Bloom | Target GPU |
|---|---|---|---|---|---|
| Low | URP-Low | 1024 | Off | Low | GTX 1050 |
| Mid | URP-Mid | 2048 | On | Mid | GTX 1060 |
| High | URP-High | 4096 | On + HQ | High | RTX 2060+ |

## 10. Coding Conventions

- C# **EditorConfig** 지정 (`.editorconfig`) — 탭 4칸·PascalCase 공개·camelCase 로컬
- 파일 1개 = 클래스 1개 원칙
- ScriptableObject로 데이터 외부화 (크리스털 정의·AI 라인 등)
- 로깅: `Debug.Log`는 개발 빌드에서만 (`#if UNITY_EDITOR || DEVELOPMENT_BUILD`)

## 11. Build Pipeline

### 11.1 1차 제출 빌드
- Target: Windows Standalone, x86_64
- Backend: IL2CPP (성능), .NET Standard 2.1
- Compression: LZ4HC
- 산출물 예상 크기: 800 MB ~ 1.2 GB
- 명명: `DDP_RecordOfLight_v1.0_Win64_YYYYMMDD.zip`

### 11.2 빌드 스크립트 (로컬)
```
# tools/build.ps1 또는 build.sh
Unity -batchmode -projectPath . -buildTarget Win64 \
      -executeMethod BuildScripts.BuildPlayer -logFile -
```

### 11.3 검증
- 최소 사양 머신(혹은 VM)에서 Boot → Ending까지 1회
- Frame time log 저장 (`Logs/frametime.csv`)
- 오디오 Peak ≤ -1 dBTP 확인 (Youlean LUFS Meter)

## 12. CI/CD (본선 확장)

- GitHub Actions: `build-win.yml` (unity-builder)
- Artifact 업로드 → itch.io butler
- Tag 기반 릴리스 (`v1.0.0-submit`)

## 13. 개발자 도구 (Editor 전용)

Scripts/Editor에 포함될 도구:
- `CrystalTable.asset` 에디터 — 각 크리스털 정의
- `BeamDebugger` — 빔 경로 시각화 오버레이
- `VignettePreview` — Game View에서 Vignette 리허설

## 14. TBD / Open Questions

| # | 질문 | 담당 | 마감 |
|---|---|---|---|
| T1 | Addressables 도입 여부 (1차 스코프) | 프로그래밍 | 2026-04-21 |
| T2 | Input rebinding 세이브 포맷 | 프로그래밍 | 2026-04-25 |
| T3 | Light Probe 배치 자동화 도구 | 프로그래밍 | 2026-04-27 |

## Change Log
- **2026-04-19** — 초안
