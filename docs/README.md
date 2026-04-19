# DDP: 빛의 기록 · 기획 문서 인덱스

> **서울 플레이업 AI 게임 챌린지 2026** 출품 기획 문서 세트
> 최종 수정: 2026-04-19

본 디렉터리는 프로덕션 운영 방식에 맞춰 관심사별로 문서를 분리한 **GDD(Game Design Document) 세트**입니다. 각 문서는 독립적으로 읽을 수 있도록 자족성을 우선하지만, 교차 참조가 필요한 경우 링크로 연결합니다.

## 읽는 순서

처음 접하는 팀원이라면 ①→② 까지가 공통 필독입니다.

### ① Vision Layer (필독)
- [00 · Pitch](00-pitch.md) — 로그라인·필러·타겟 경험. 먼저 여기부터
- [01 · Game Design](01-game-design.md) — 코어 루프·메커닉·난이도 곡선
- [02 · Narrative](02-narrative.md) — 세계관·캐릭터·6개 크리스털 서사

### ② Production Layer
- [03 · Level Design](03-level-design.md) — 3존 레벨 구조와 비트 차트
  - [levels/zone1-alimter.md](levels/zone1-alimter.md)
  - [levels/zone2-baeumter.md](levels/zone2-baeumter.md)
  - [levels/zone3-salimter.md](levels/zone3-salimter.md)
- [04 · Art Direction](04-art-direction.md) — 비주얼 바이블
- [05 · Audio Direction](05-audio-direction.md) — 사운드·음성·믹스
- [06 · UI/UX](06-ui-ux.md) — 디제틱 지향 UI 설계
- [07 · Technical Design](07-technical-design.md) — 아키텍처·퍼포먼스 예산
- [08 · VARCO Pipeline](08-varco-pipeline.md) — VARCO 3D/Sound/Voice 통합

### ③ Delivery Layer
- [09 · Production Plan](09-production-plan.md) — 스프린트·DoD·마일스톤
- [10 · Submission](10-submission.md) — 제출물 사양·체크리스트
- [11 · Risks](11-risks.md) — 리스크 레지스터

### ④ Environment Setup
- [UNITY_SETUP.md](UNITY_SETUP.md) — Unity 최초 오픈 7단계 (macOS 중심)
- [WINDOWS_WSL2_SETUP.md](WINDOWS_WSL2_SETUP.md) — Windows + WSL2 이어받기 원샷 가이드

## 문서 작성 규칙

1. **권위 문서 (Source of Truth)** — 본 세트는 프로젝트 내 모든 설계 결정의 근거. Slack·이메일 논의 결과는 해당 문서에 반영해야 확정된 것으로 간주
2. **변경 로그** — 주요 변경은 각 문서 최하단 `## Change Log`에 YYYY-MM-DD 단위로 추가
3. **명세 수준** — 모든 수치는 단위와 함께 (예: `이동 속도 3.5 m/s`, `FOV 70°`). 추상 서술 금지
4. **금지** — "적절한", "자연스러운", "추후 정의" 등의 표현은 값 확정 전까지 `TBD (owner, due date)` 형태로 기재
5. **다이어그램** — 텍스트(ASCII/Mermaid) 우선, 필요 시 PNG는 `diagrams/`에 배치

## 용어집 (Glossary)

| 용어 | 정의 |
|---|---|
| **Crystal** | 메모리 크리스털. 복원 대상 핵심 오브젝트. 색상 요구 조건을 만족하는 빛이 닿으면 활성화 |
| **Prism** | 플레이어가 배치하는 빛 굴절/반사 오브젝트 |
| **Node** | 프리즘 배치 가능 지점 (월드에 고정된 스냅 슬롯) |
| **Beam** | 광원에서 발사되어 프리즘·크리스털과 상호작용하는 빛의 선 |
| **Zone** | 레벨 단위. 본작은 Zone 1/2/3 구성 |
| **Vignette** | 크리스털 활성화 시 재생되는 1인칭 홀로그램+보이스 단편 |
| **Archive (아카이브)** | 율의 이어셋에 상주하는 가이드 AI. VARCO Voice로 음성 구현 |
| **Yul (율)** | 플레이어 캐릭터. 21세, 도시 기억 아카이브 수습 연구원 |

## Change Log
- **2026-04-19** — 초안 작성 (13문서 세트 초기화)
