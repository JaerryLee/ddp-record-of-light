# 04 · Art Direction

> 본 문서는 **비주얼 바이블**. 모든 3D 에셋·머티리얼·라이팅·VFX 판단의 기준.

## 1. Mood Statement

> *"꺼진 도시, 깨어나는 빛. 사람의 체온만큼 차갑고 따뜻한 SF."*

차가운 미래가 아니라 **조용한 미래**다. 디테일은 서울적이고 구체적이되, 전체 인상은 여백이 넓고 고요하다. DDP의 곡면이 화면의 뼈대가 되고, 빛이 그 위에 문장을 쓴다.

## 2. Visual Pillars

1. **곡면과 빛 (Curves & Light)** — 직선은 최소화. 모든 레벨 지오메트리는 곡률을 1회 이상 포함
2. **앰버 × 네이비 (Amber × Navy)** — 차가운 배경 위의 따뜻한 포인트. 3:97 비율을 지향
3. **절제된 디테일 (Quiet Detail)** — 모든 프롭은 1개 이상의 의미를 가진다. 장식용 프롭 금지

## 3. Color Palette

### 3.1 Primary
| 이름 | Hex | 사용처 | 비중 |
|---|---|---|---|
| Deep Navy | `#1A1F35` | 배경·외벽 음영 | 45% |
| Cool White | `#E8EEF5` | 라이트·파노라마 하늘 | 25% |
| Amber | `#FFB547` | 빛·크리스털 기본·UI 포인트 | 8% |
| Crystal Cyan | `#6FDBFF` | 크리스털 코어·아카이브 rim | 5% |
| Shadow Black | `#0B0D18` | 심부 그림자 | 17% |

### 3.2 색 배분 규칙 (60-30-10 변형)

이 게임은 **45-25-30** 구조를 사용:
- 45% 배경(Navy 계열) · 25% 미드톤(Gray·White) · **30%는 빛과 공백** 
- 빛은 "덩어리"가 아니라 **선(빔)**과 **점(크리스털)**으로만 등장

### 3.3 색 사용 금지 규칙
- 완전 순수 채도(#FF0000 등) 금지. 모든 색은 최소 10% 블랙 블렌드
- 남색 계열 제외한 **푸른 배경 금지** (청색은 크리스털 액센트 전용)
- 녹색은 Zone 3 C5 퍼즐에서만 등장 (감정 키 컬러)

## 4. Lighting Rules

### 4.1 철칙
- **실시간 그림자는 메인 광원 1개만**. 나머지는 베이크
- **Exposure 고정**. 자동 노출 금지 — 의도된 어둠이 "밝아지면" 안 됨
- **Bloom intensity ≤ 0.6**. 빛이 뭉개지지 않게

### 4.2 톤매핑
- ACES Filmic
- Color Grading: LUT 2장 준비 (Baeumter_Cool.cube, Salimter_Warm.cube)
- White Balance: 6200K (중립)

### 4.3 씬별 광도 목표 (EV100)
| 씬 | EV100 | 비고 |
|---|---|---|
| Zone 1 (광장·야외) | 6~8 | 비상등 앰버 중심 |
| Zone 2 (실내) | 4~6 | 조명 기둥 인근 +2 |
| Zone 3 (옥상) | 9~11 | 달빛·도시광 |
| Vignette 홀로그램 | 주변 -2 | 덕킹 |

## 5. Material Philosophy

### 5.1 PBR 프리셋
- URP Lit 셰이더 기본
- Metallic: 대부분 0 또는 1 (중간값 금지)
- Smoothness: 0.2~0.9. 0 또는 1 금지 — "죽은 표면" 회피
- Normal: 강도 0.5~1.0. 과한 디테일 노멀 금지

### 5.2 재질 계층
| 레이어 | 용도 | 예시 |
|---|---|---|
| Base | 대부분의 환경 | 콘크리트·금속 판재 |
| Glow | 크리스털·광원 | Emissive = color × 3.0 |
| Semi-transparent | 홀로그램 | Alpha 0.4~0.6 + Rim |
| Liquid | 없음 | (수면·유체 에셋 제외) |

### 5.3 셰이더 예산
- 커스텀 셰이더 ≤ **6개**: `Beam`, `CrystalCore`, `Hologram`, `PrismRefract`, `RimEdge`, `SkyboxNight`

## 6. Character & Hologram Style

- **풀바디 캐릭터 없음** (1인칭). 플레이어 팔/손만 등장 가능성 있으나 1차는 **손 미노출**
- **홀로그램 실루엣**: 인물 풀 메쉬 없이 **로우폴리 + 반투명 + 강한 림 라이트**
  - 버텍스 ≤ 3k / 실루엣
  - 움직임은 짧은 루프 애니 (1~2초)

## 7. Asset Quality Targets

| 에셋 유형 | 트라이앵글 상한 | 텍스처 맥스 | LOD |
|---|---|---|---|
| Hero Prop (프리즘·크리스털) | 8k | 2048×2048 | 2 |
| 일반 Prop | 3k | 1024×1024 | 2 |
| Modular 건축 조각 | 5k | 2048 (tile-able) | 2 |
| 홀로그램 실루엣 | 3k | 512 | 0 |
| VFX 쿼드 | 6 quads 이하 | 256 | 0 |

## 8. VFX Catalog

| VFX | 용도 | 구현 |
|---|---|---|
| BeamLine | 빔 시각화 | LineRenderer + Emissive 머티리얼 + Additive |
| BeamImpact | 빔이 표면에 닿는 지점 | Particle Quad (short lifetime) |
| CrystalAwaken | 크리스털 활성화 | Sphere burst + Radial rays + SFX sync |
| HologramFlicker | 홀로그램 생성/소멸 | Dissolve shader + noise mask |
| LeafFall (Zone 3) | 낙엽 | Particle, wind force |
| BackgroundDust | 실내 미세 먼지 | Particle, low density |

## 9. Reference Moodboard (키워드)

- 자하 하디드 DDP 내부 야간 사진
- Playdead INSIDE (공간의 침묵)
- Annapurna Tacoma (홀로그램 UI)
- 영화 *Her* (2013) 실내조명
- 영화 *Arrival* (2016) 엔딩 색감
- thatgamecompany *Journey* 끝자락 광원
- 사진가 Todd Hido (조용한 야간 건축)

> 참고 이미지는 `docs/diagrams/moodboard/`에 저작권 추적용 출처와 함께 배치 (외부 공개물엔 미포함).

## 10. 제작 가이드 (VARCO 3D 아티스트용)

1. **프롬프트 언어**: 영문 우선. 한국어 보조
2. **에셋당 뷰**: 정면·3/4 회전 ·상단 최소 3컷 생성 후 베스트 1개 선정
3. **토폴로지**: VARCO 3D 결과물은 대체로 트라이 기반. **Blender에서 Quad 리토폴로지** 1패스 필수
4. **UV**: VARCO 기본 UV 사용 가능. 겹침 발견 시 재전개
5. **머티리얼**: VARCO 생성 머티리얼은 참고용. **본 프로젝트 머티리얼 라이브러리**로 재매핑 필수

## 11. 허용/금지 목록

### Do
- DDP의 **곡면 그 자체**를 반복 모티프로
- 빛은 **얇고 명확한 선**으로
- 어둠은 **디테일이 살아 있는 어둠** (pure black 회피, 블루 미드톤 유지)

### Don't
- 실존 브랜드 로고·포스터 직접 재현
- 네온 사이버펑크 비주얼
- 필름 그레인 남발
- 렌즈 플레어 과다
- 화면 상하 블랙바(시네마틱 빙자)

## 12. 변경 거버넌스

아트 방향 변경은 **디렉터 결재** 필수:
1. 본 문서에 Pull Request 또는 Change Log 추가
2. 아트 파이프라인 영향 평가
3. 디렉터 OK 후 반영

## Change Log
- **2026-04-19** — 초안
