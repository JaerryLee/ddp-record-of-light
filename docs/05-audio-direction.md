# 05 · Audio Direction

> 음성 스크립트는 [02 · Narrative](02-narrative.md), VARCO Sound/Voice 파이프라인은 [08](08-varco-pipeline.md). 본 문서는 **톤·믹스·파일 사양**을 담는다.

## 1. Audio Philosophy

> *"침묵은 적이 아니라 악기다."*

빈 공간을 두려워하지 않는다. 앰비언스 레벨은 -30 LUFS까지 낮아지는 구간이 존재해야 하며, 주요 이벤트는 그 고요함 위에 놓일 때 의미를 얻는다.

## 2. Three Layers

게임 오디오는 항상 3개 레이어로 구성된다.

| 레이어 | 우선도 | 예시 | 평균 볼륨 |
|---|---|---|---|
| **Ambience** (베드) | Low | 공조음, 바람, 실내 험 | -28 ~ -22 LUFS |
| **Interaction** | Mid | 프리즘 설치, 빔 연결, 크리스털 깨어남 | -18 LUFS |
| **Narrative** | High | Archive AI, 크리스털 Vignette, 엔딩 내레이션 | -14 LUFS |

보이스 재생 중엔 Ambience · BGM을 **-6 dB ducking**. SFX는 -3 dB ducking.

## 3. Music Plan

### 3.1 트랙 리스트
| ID | 사용 | 길이 | 루프 | 악기 구성 |
|---|---|---|---|---|
| `MUS_Intro` | 타이틀~오프닝 | 45s | ❌ | Pad + Piano |
| `BGM_Alimter` | 없음 (자연음만) | - | - | - |
| `BGM_Baeumter` | Z2 전체 | 4m 루프 | ✅ | Piano + 미묘한 Pad |
| `BGM_Salimter_Ambient` | Z3 ~C6 전 | 5m 루프 | ✅ | Pad + 먼 현 |
| `BGM_Ending_Theme` | C6 활성화~엔딩 | 3m 30s | ❌ | Piano + 국악 현(가야금) + Pad |
| `MUS_Credits` | 크레딧 | 3m | ❌ | Piano solo |

### 3.2 작곡 가이드라인
- **Key**: Dm / Am 중심의 마이너 기반. Major는 엔딩 후반에만
- **BPM**: 60~72 (호흡 속도)
- **Dynamic Range**: 넓게 유지 (-3 LUFS 피크 ~ -24 LUFS 계곡)
- **VARCO Sound** 가능 영역: BGM_Baeumter, BGM_Salimter_Ambient, BGM_Ending_Theme
- VARCO로 부족한 경우 CC0 또는 커스텀 커미션 허용. 단 **엔딩 테마는 오리지널** 필수 (저작권 명확)

## 4. SFX Design

### 4.1 카테고리

| 카테고리 | 예시 | 톤 |
|---|---|---|
| **Prism** | 설치, 회수, 회전 | 짧고 명료. 고역 "틱"·"클리크" |
| **Beam** | 생성, 연결, 끊김 | 지속 hum. 색마다 미묘하게 다른 톤 (R=0.9x, G=1.0x, B=1.1x pitch) |
| **Crystal** | 활성화, 맥동, 꺼짐 | 차임 계열 + 서브 베이스 "쿠웅" |
| **UI** | 메뉴 이동, 확인 | 최소한. 디제틱 느낌 |
| **Environment** | 바람, 걸음, 문 | 재질별 3종 이상 |
| **Hologram** | 생성, 소멸 | 디지털 글리치 + 에어 텍스처 |

### 4.2 Footstep Rules
- 재질 감지: Plaza(시멘트), Indoor(마블), Grass(잔디), Catwalk(금속)
- 각 재질당 **4개 랜덤 샘플** + pitch ±5%
- 속도: 걷기만 존재 (이동 속도 3.5 m/s) → 스텝 간격 0.55초

### 4.3 파일 사양
- 포맷: **WAV 16-bit 44.1kHz** (원본), Unity import 시 **Vorbis 품질 70**
- 명명: `SFX_<Category>_<Action>_<Variant>.wav` 예: `SFX_Prism_Place_A.wav`
- 최대 길이: SFX 3초, 차임 6초

## 5. Voice Direction

### 5.1 Archive AI

| 속성 | 값 |
|---|---|
| 성별·나이 | 여, 20대 후반 ~ 30대 초반 |
| 음색 | 낮고 따뜻. 숨소리 포함 |
| 말속도 | 한국어 기준 평균 3.5~4 글자/초 (느린 편) |
| 공간감 | 이어셋 내장 → **드라이 + 살짝의 close-mic reverb** |
| 감정 변화 | 최소 3종 (중립·경이·회한) |
| VARCO Voice 시드 | 1회 확정 후 전 게임 재사용 (일관성) |

### 5.2 크리스털 화자

각 크리스털은 **다른 보이스 시드**. 6개 다양성 확보가 핵심.

| 크리스털 | 보이스 특성 | 환경 처리 |
|---|---|---|
| C1 | 30대 남, 공식 톤 | 외부 PA 에코 |
| C2 | 30대 여 + 7세 아이 | 야외 룸 톤 |
| C3 | 40대 여, 속삭임 | 작은 룸 + 바람 |
| C4 | 50대 남, 피로감 | 실내, 약간의 공조음 |
| C5 | 20대 여 + 20대 남 | 강한 야외 바람 |
| C6 | 40대 여, 긴장→체념 | 룸 톤 + 긴급 경보 원거리 |

### 5.3 레코딩/생성 규칙
- VARCO Voice 원본은 **저장 후 Audition/Reaper에서 노말라이즈** (-1 dBFS peak, -16 LUFS target)
- 노이즈 게이트: -60 dB 이하 컷
- De-esser: 경미하게, 자연스러움 우선
- **한국어 자연스러운 억양이 어색할 경우 VARCO Voice 재생성** (3회 시도 후 최상) — 절대 문장 전체를 그대로 쓰지 말 것

## 6. Mix & Mastering

### 6.1 믹서 구조 (Unity AudioMixer)

```
Master (-6 dB headroom)
 ├─ Music (-2 dB)
 ├─ SFX (+0 dB)
 │   ├─ Prism
 │   ├─ Beam
 │   ├─ Crystal
 │   ├─ Environment
 │   └─ UI
 ├─ Voice (+2 dB)
 │   ├─ Archive
 │   └─ Vignette
 └─ Ambience (-4 dB)
```

### 6.2 Snapshot (동적 믹스)
- `Default` — 일반 탐험
- `VoiceActive` — 보이스 재생 중 (Ambience -6, Music -6, SFX -3)
- `CrystalActivation` — 활성화 버스트 (SFX +4, 1초 후 Default 복귀)
- `EndingCinematic` — 엔딩 (Voice +2, 그 외 -4)

### 6.3 Loudness Target
- 전체 빌드 평균 **-16 LUFS integrated**
- 트루 피크 ≤ **-1 dBTP**
- 엔딩 시퀀스 peak은 -14 LUFS까지 허용

## 7. Subtitles

- 전 음성 **자막 기본 ON**
- 폰트: Pretendard Medium (한)·Inter Medium (영)
- 크기 3단계: Small(18px)·Default(22px)·Large(28px)
- 배경: 반투명 검정 75% → 가독성
- 화자 색 구분: Archive AI = Amber, 크리스털 화자 = Cool White

## 8. Audio Inventory

| 카테고리 | 예상 파일 수 | 총 길이 | 출처 |
|---|---|---|---|
| BGM | 6 | 약 21분 | VARCO Sound + 오리지널 |
| SFX | 약 50 | 약 3분 | VARCO Sound + CC0 |
| Voice - Archive | 28 | 약 5분 | VARCO Voice |
| Voice - Vignette | 7 | 약 3분 | VARCO Voice |
| Ambience | 6 | 약 30분 (루프) | VARCO Sound |
| **합계** | **약 97 파일** | **약 60분** | |

## 9. 폴더/이름 규칙

```
Assets/_Project/Audio/
├── BGM/        MUS_*, BGM_*
├── SFX/        SFX_<Cat>_<Action>_<Var>
├── Voice/
│   ├── Archive/        VO_Archive_<SceneID>_<LineID>
│   ├── Vignettes/      VO_Crystal_C<N>_<LineID>
│   └── scripts/        *.json (대사 스크립트)
├── Ambience/   AMB_<Zone>_<Layer>
└── Mixers/     Main.mixer, snapshots.asset
```

## 10. 체크리스트

- [ ] 전 씬 -16 LUFS integrated 도달
- [ ] 자막 토글이 모든 보이스에 적용
- [ ] VoiceActive 스냅샷 자연 전환 (50ms 페이드)
- [ ] 엔딩 오디오가 블랙 아웃 이후에도 끊김 없이 이어짐
- [ ] 모든 보이스 파일에 VARCO export ID 기록

## Change Log
- **2026-04-19** — 초안
