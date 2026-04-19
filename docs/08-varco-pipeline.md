# 08 · VARCO Pipeline

> 본작의 심사 가산점 핵심. **VARCO 3D / Sound / Voice** 3종을 모두 활용하고, 심사 증빙에 쓸 수 있도록 **Manifest 중심 운영**을 한다.

## 1. Tool Matrix

| 도구 | 사용처 | 1차 비중 목표 |
|---|---|---|
| **VARCO 3D** | 환경 소품·크리스털·프리즘·홀로그램·건축 디테일 파츠 | ≥ **65%** (공모 기준 50% 상회) |
| **VARCO Sound** | BGM 3곡, 앰비언스 6레이어, SFX 약 20종 | 약 70% |
| **VARCO Voice** | Archive AI 28라인 + Vignette 6 + 엔딩 내레이션 | **100%** |

## 2. 3D Pipeline

### 2.1 프로세스 (생성부터 씬 배치까지)

```
 ① Prompt 작성 (영문)
     ↓
 ② VARCO 3D 생성 (정면/3-4/상단 3컷 이상)
     ↓
 ③ 최상 품질 1컷 선정 → GLB/FBX Export
     ↓
 ④ Blender 정리 (리토폴로지·UV 검수·오리진 위치)
     ↓
 ⑤ Unity Import (Assets/_Project/Art/...)
     ↓
 ⑥ 프로젝트 머티리얼 라이브러리 재매핑
     ↓
 ⑦ Manifest 등록 (varco_manifest.csv)
     ↓
 ⑧ Prefab 제작
```

### 2.2 Prompt 가이드라인 (에셋 아티스트용)

Good:
> `"A sleek, cyan-glowing crystal memory module, hexagonal base, soft inner light pulsing, PBR materials, game-ready, 2048 texture, neutral pose, centered."`

Bad:
> `"memory crystal"` (너무 추상, VARCO가 장식 스타일로 변질)

### 2.3 정리 규칙 (Blender)
- **Apply All Transforms** 후 export
- **Origin at Base**: 스케일 1 m = 1 유닛 기준 바닥 중심
- **UV0 리셋**: VARCO 기본 UV 재사용 가능. 겹침 발견 시 Smart UV Project
- **Materials 제거** — Unity에서 프로젝트 머티리얼 라이브러리 재매핑

### 2.4 Unity Import 설정
- Model Tab: Scale 1.0, Read/Write OFF
- Rig Tab: None (비캐릭터)
- Animation Tab: OFF
- Materials Tab: Use External Materials (Legacy) → 이후 재매핑

## 3. Sound Pipeline

### 3.1 BGM
- VARCO Sound "instrumental / cinematic / piano & pad / minor key" 프롬프트로 생성
- 길이: 루프 4분 이상 요청
- 결과물은 **DAW에서 루프 포인트 편집** (Audacity or Reaper). 파일명 `BGM_*.wav`

### 3.2 Ambience
- VARCO Sound "room tone / HVAC / subtle city hum / wind high altitude" 등 개별 생성
- 레이어 2~3개 겹치기: Unity AudioSource + 미묘한 loop offset

### 3.3 SFX
- 짧은 "click", "chime", "hum", "pop" 계열은 VARCO Sound 다수 생성 후 엄선
- 복잡한 물리 음(낙엽·걸음)은 CC0 라이브러리(Freesound) 혼용 허용. 단 **라이선스 표기 필수**

## 4. Voice Pipeline

### 4.1 스크립트 준비
- 대사 원고는 [02 · Narrative §4](02-narrative.md)에 JSON으로 버전 관리
- 한/영 동시 입력. 방언·감정 태그는 `direction` 필드에 기재

### 4.2 VARCO Voice 생성 절차
1. 스크립트 `korean` 필드 복사
2. VARCO Voice 대시보드에서 **보이스 시드 선택**
3. 감정 태그 적용 (중립/회한/경이 등)
4. 생성 결과 3회 샘플 → 최상 1개 선정
5. Export WAV 24-bit 48kHz (후처리 헤드룸 확보)

### 4.3 후처리 (DAW)
- Loudness normalize: -16 LUFS integrated
- De-esser: 고역 치찰음 경미
- Light compression: 2:1, 소스 자연스러움 유지
- High-pass filter 80 Hz

### 4.4 Archive AI 일관성
- **보이스 시드는 1회 확정 후 전 게임 재사용**
- 시드 변경은 디렉터 승인 필요 (감정 일관성 훼손 시)

## 5. Asset Naming & Folder Structure

```
varco_source/                        ← 원본 export (LFS)
├── 3d/
│   ├── crystals/
│   ├── prisms/
│   ├── environment/
│   └── props/
├── sound/
│   ├── bgm/
│   ├── ambience/
│   └── sfx/
└── voice/
    ├── archive/
    └── vignettes/

Assets/_Project/Art/...              ← Unity-imported (정리본)
Assets/_Project/Audio/...            ← 후처리 완료본
```

## 6. VARCO Manifest

### 6.1 파일 위치
- `docs/varco_manifest.csv` (심사 증빙)
- 동시에 `tools/varco_manifest.csv`에 운영 사본

### 6.2 스키마

```csv
id,type,name,scene_use,prompt,created_at,export_id,file_path,notes
E001,3d,Crystal_C1,Zone1,"A sleek cyan-glowing crystal module, hex base, PBR",2026-04-22,VARCO-3D-xxxxx,Assets/_Project/Art/Props/Crystal_C1.prefab,
S001,sound,BGM_Baeumter,Zone2,"Minimal piano with warm pad, Dm, 60bpm, 4min loop",2026-04-25,VARCO-SND-xxxxx,Assets/_Project/Audio/BGM/BGM_Baeumter.wav,
V001,voice,VO_Archive_Intro_01,Intro,"Korean, female mid-20s, warm calm",2026-04-23,VARCO-VC-xxxxx,Assets/_Project/Audio/Voice/Archive/VO_Archive_Intro_01.wav,
```

### 6.3 자동화
- `tools/scripts/generate_manifest.py` (본선 확장)
- 1차는 수기 입력 + 생성시 즉시 기록 원칙

## 7. 증빙 패키지 (제출물)

심사에 제출할 VARCO 활용 증빙:

| 산출물 | 설명 |
|---|---|
| `varco_manifest.csv` | 전 에셋 목록 |
| `docs/evidence/before_after/*.png` | Before/After 비교 스크린샷 10~15장 |
| `docs/evidence/clips/*.mp4` | VARCO 생성 화면 녹화 5종 (각 20~40초) |
| `docs/evidence/prompts_log.md` | 대표 프롬프트 20개 모음 |
| `docs/evidence/usage_report.md` | 활용 비중 정량 요약 (3D 65% 등 근거) |

### 7.1 Before/After 예시 목록 (기본 10개)
1. Crystal_C1 · VARCO raw → 정리본
2. Prism_Splitter · VARCO raw → 게임 씬
3. Archive_Rack_A · 프롬프트 입력 → 결과 → 씬 배치
4. Banner_Faded · 원본 → 먼지 디테일
5. Memory_Light_Pillar · 반복 배치 전후
6. BGM_Baeumter · 원본 루프 → 후처리 파형
7. AMB_Salimter_Wind · 원본 → 루프 최적화
8. VO_Archive_Intro · raw 생성 → 자연스러움 보정
9. VO_Crystal_C3 · 3회 생성 → 최종 선택 근거
10. VO_Ending · 최종 내레이션 스펙트럼

## 8. 운영 체크포인트

| 날짜 | 체크 | 담당 |
|---|---|---|
| 2026-04-24 | 3D 10개·Voice 2개·Sound 2개 manifest 완료 | 아트/오디오 |
| 2026-04-28 | 3D 30개·Voice 5개·Sound 6개 | 아트/오디오 |
| 2026-05-02 | 3D ≥ 목표 비중, Voice 전체, Sound 전체 | 디렉터 확인 |
| 2026-05-05 | Before/After + 녹화 클립 완료 | 디렉터 |
| 2026-05-06 | 증빙 패키지 zip화 | 디렉터 |

## 9. 리스크 플래그

- **퀄리티 편차**: VARCO 3D 일부 결과물의 토폴로지가 깨지는 경우 → Blender 재작업 1일 버퍼 확보
- **Voice 자연성**: 한국어 억양 어색할 때 **스크립트 리라이팅** 허용 (문어체 → 구어체 교체)
- **저작권**: VARCO 산출물의 상업 이용 라이선스 재확인. 공모전 규정상 홍보 목적은 양도

## 10. 다른 부문 (2D) 대응 참고

본작은 3D 부문이지만 **Sound/Voice 100% VARCO** 조건은 2D 부문 기준도 충족한다. 방향 전환 가능성에 대비한 안전판.

## Change Log
- **2026-04-19** — 초안
