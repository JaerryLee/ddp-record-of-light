# 09 · Production Plan

> 제출 마감: **2026-05-07** · 전체 기간 **18일** · 본 문서는 **하루 단위 스프린트 백로그**까지 정의한다.

## 1. Milestones

| 마일스톤 | 날짜 | 정의 |
|---|---|---|
| **M0 — Kick Off** | 2026-04-19 | 기획 문서 세트 초안 완료, 리포 부트스트랩 |
| **M1 — Playable Prototype** | 2026-04-24 | Zone 1 전체 플레이 가능 (보이스 제외) |
| **M2 — All Zones Whitebox** | 2026-04-28 | Zone 1~3 전체 퍼즐 풀이 가능 |
| **M3 — Art & Audio Pass** | 2026-05-02 | 모든 VARCO 에셋 배치, 첫 QA 사이클 통과 |
| **M4 — Feature Freeze** | 2026-05-04 | 기능 변경 중단. 폴리싱·버그픽스만 |
| **M5 — Submission Candidate** | 2026-05-06 | 모든 제출물 드래프트 완성 |
| **M6 — Submit** | 2026-05-07 | 최종 업로드 |

## 2. Sprint Breakdown

### Sprint 0 — 2026-04-19 (Sat)
**목표**: 리포·문서·Unity 프로젝트 셋업
- [x] 기획 문서 세트 13종 초안 작성
- [x] 리포 부트스트랩 (Assets 구조, .gitignore, .gitattributes)
- [ ] git init + 원격 연결
- [ ] Unity Hub에서 본 폴더를 Unity 2022.3 LTS 프로젝트로 Add
- [ ] URP 템플릿 적용, Settings 폴더 구성
- [ ] New Input System 설치·Player.inputactions 스캐폴드

### Sprint 1 — 4/20(Sun) ~ 4/23(Wed) · 4일 · "**프로토타입**"
**목표**: 코어 메커닉 작동 (그레이박스 Zone 1)

| Day | Task | DoD |
|---|---|---|
| 4/20 Sun | PlayerController 이동·카메라·인터랙션 레이 | 3.5 m/s, FOV 70, 마우스 감도 옵션 |
| 4/21 Mon | PrismNode + PrismInstance 배치 시스템 | E 키로 스냅 배치·회수 |
| 4/22 Tue | BeamResolver (반사 3회, LineRenderer 시각화) | 벽·크리스털 히트 콜백 동작 |
| 4/23 Wed | MemoryCrystal 활성화 → VignettePlayer 스켈레톤 | C1 더미 보이스 재생 가능 |

### Sprint 2 — 4/24(Thu) ~ 4/27(Sun) · 4일 · "**VARCO 1차 + Zone 2**"
**목표**: VARCO 에셋 초기 생성, Zone 2 그레이박스

| Day | Task | DoD |
|---|---|---|
| 4/24 Thu | VARCO 3D: Prism/Crystal/핵심 Prop 10종 생성·정리·Unity Import | Manifest 10건 등록 |
| 4/25 Fri | VARCO Voice: Archive AI 오프닝 + C1 대사 녹음·후처리 | 2개 라인 게임 내 재생 |
| 4/26 Sat | Splitter/Filter 프리즘 구현 (색 마스크, 3분기) | Zone 2 색 퍼즐 기본 통과 |
| 4/27 Sun | Zone 2 화이트박스 + P2.1, P2.2 배선 | 풀이 가능 |

### Sprint 3 — 4/28(Mon) ~ 5/1(Thu) · 4일 · "**Zone 3 + 콘텐츠**"
**목표**: 전 존 퍼즐 완성, VARCO 에셋 2차 생성

| Day | Task | DoD |
|---|---|---|
| 4/28 Mon | Zone 3 화이트박스 + Mirror+ 회전 구현 | P3.1 기본 통과 |
| 4/29 Tue | C6 시간제 퍼즐 + 엔딩 시퀀스 스크립트 | 엔딩 시점 정상 트리거 |
| 4/30 Wed | VARCO 3D 30개 추가 (아카이브 랙, 옥상 언덕, 포탈 조형) | Manifest 40+ |
| 5/1 Thu | VARCO Voice 나머지 라인 (C3~C6, 엔딩) | 전체 보이스 인게임 재생 |

### Sprint 4 — 5/2(Fri) ~ 5/5(Mon) · 4일 · "**폴리싱**"
**목표**: 라이팅·사운드·최적화, 1차 QA

| Day | Task | DoD |
|---|---|---|
| 5/2 Fri | 라이팅 베이크 전 존 + 포스트 프로세싱 | 씬 로드 후 bake 재수행 불필요 |
| 5/3 Sat | 오디오 믹스 · 스냅샷 설정 · LUFS 통과 | -16 LUFS integrated |
| 5/4 Sun | 성능 최적화 (드로우콜·셰이더·LOD) | GTX 1060 60fps 평균 |
| 5/5 Mon | QA 라운드 1 (3명), 버그 픽스 | P0 버그 0건 |

### Sprint 5 — 5/6(Tue) ~ 5/7(Wed) · 2일 · "**제출**"
**목표**: 빌드·영상·문서 마감

| Day | Task | DoD |
|---|---|---|
| 5/6 Tue 오전 | Win64 빌드 생성 · 최소 사양 머신 검증 | zip ≤ 1.5 GB, 완주 OK |
| 5/6 Tue 오후 | 시연 영상 촬영 (OBS) → 편집 (DaVinci) | 5~8분, 1080p60 |
| 5/6 Tue 밤 | 게임 설명 PDF + 2차 계획서 + VARCO 증빙 패키지 | 모든 제출물 zip화 |
| 5/7 Wed 오전 | 최종 업로드 | 제출 확인 메일 수신 |
| 5/7 Wed 오후 | 예비 — 재업로드·보완 | - |

## 3. Definition of Done

### 3.1 에셋 DoD
- VARCO 3D: Manifest 등록 + 프로젝트 머티리얼 적용 + LOD 2단 + Prefab화
- VARCO Sound: 정규화 -16 LUFS, 루프 포인트 검증
- VARCO Voice: De-essed·정규화 + JSON 스크립트 매칭 + 자막 일치

### 3.2 기능 DoD
- 단위 테스트 혹은 수동 재현 절차 3회 통과
- QA 체크리스트 해당 항목 ✅
- 콘솔 에러·워닝 0건 (프로덕션 빌드 기준)

### 3.3 스프린트 DoD
- M0~M6 각 마일스톤별 체크리스트 100% 통과
- `docs/`에 해당 스프린트 변경 사항 Change Log 업데이트

## 4. Daily Cadence

- **09:00** — 하루 시작, 전일 완료·오늘 목표 스스로 체크 (솔로 스탠드업)
- **12:30** — 점심 + 휴식 30분
- **18:00** — 오늘 산출물 git commit + manifest 업데이트
- **22:00 전** — 하루 마감. 과로 금지 (버그 밀도 증가 원인 1위)

## 5. Tooling & Environment

| 목적 | 도구 |
|---|---|
| 에디터 | Unity 2022.3 LTS · JetBrains Rider (or VSCode) |
| 3D 정리 | Blender 4.0+ |
| 오디오 | Reaper · iZotope RX (선택) · Youlean LUFS Meter |
| 녹화 | OBS Studio |
| 편집 | DaVinci Resolve 18 |
| 버전관리 | Git + LFS, GitHub |
| 이슈 트래킹 | GitHub Issues (1차) |
| 기획 변경 | `docs/`에 PR로 관리 |

## 6. 팀 구성 & 역할

| 역할 | 인원 | 담당 |
|---|---|---|
| Director / Lead Programmer | 1 | 전체 디렉션, 코어 시스템, 레벨 배선 |
| Tech Art / Asset Integration | 1 (본인 겸임) | VARCO 3D 정리, 셰이더, 라이팅 |
| Audio | 1 (본인 겸임) | VARCO Sound/Voice 후처리, 믹스 |
| QA | 2 (외부 지인) | Sprint 4, 1회 |

> 1인 팀 전제. 본선 진출 시 3~4인 확장 계획은 [10 · Submission §2차 계획](10-submission.md) 참조.

## 7. Risk-Adjusted Buffer

전 스프린트 작업량은 **실 가용 시간의 80%**로 산정. 나머지 20%는:
- 버그 픽스 · VARCO 재생성 · QA 피드백 반영용

예: 하루 가용 10시간 → 백로그 8시간 기준 계획, 2시간 버퍼.

## 8. 의사소통

- 문서 기반: `docs/` Pull Request
- 논의 필요 이슈: GitHub Issues 라벨 `discussion`
- 최종 승인: 디렉터 (본인) 주석 확정

## 9. 마일스톤별 시연 가능 범위

| M | 시연 가능한 것 |
|---|---|
| M1 | Zone 1 끝까지 플레이, 보이스 없음, 임시 에셋 |
| M2 | Zone 3 엔딩까지 플레이, 보이스 일부, 임시 에셋 |
| M3 | 모든 에셋·보이스 포함, 라이팅 미완 |
| M4 | 최종 퀄리티 근접, 본선 영상 촬영 가능 |
| M5 | 제출 후보 빌드 |
| M6 | 제출 최종 빌드 |

## 10. 일정 변동 대응 절차

1. Day N에 Day N-1 스프린트 항목 미달 시
2. **오늘 새 작업 시작 전** 리스크 평가: 단순 지연인가, 스코프 조정인가?
3. 스코프 조정일 경우 즉시 **컷 후보** 적용 (본 문서 §11)
4. 문서에 Change Log

## 11. 컷 후보 (스코프 축소 예비 플랜)

제출 압박 심해질 경우 아래 순서로 컷:
1. Zone 3 **추가 환경 디테일** (언덕 외 시설물)
2. **포토 모드** F12
3. **영어 자막** (한국어 자막만 1차 제출)
4. Zone 2 **Splitter/Filter 분리** → 하나로 통합
5. (마지막) Zone 3 C6 시간제 → 단순 정적 퍼즐로 대체

하지만 절대 컷 금지:
- 엔딩 시퀀스
- VARCO 활용 50% 기준
- QA 라운드 1회 이상

## Change Log
- **2026-04-19** — 초안
