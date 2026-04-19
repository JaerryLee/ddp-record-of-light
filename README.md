# DDP: 빛의 기록 (DDP: Record of Light)

> 제1회 **서울 플레이업 AI 게임 챌린지** (2026) 출품작
> 장르: 3D 1인칭 퍼즐-탐험 · 플랫폼: PC(Windows) · 엔진: Unity 2022 LTS(URP)

2045년 대정전 이후의 DDP. 플레이어는 서울의 기억을 빛으로 복원하는 수습 연구원이 되어, 곡면 공간을 프리즘으로 관통시켜 메모리 크리스털을 되살린다.

---

## Quick Links

- 📘 **[기획 문서 전체 보기](docs/README.md)** — Pitch · Game Design · Narrative · Levels · Art · Audio · UI/UX · Technical · VARCO · Production · Submission · Risks
- 🗓 **[개발 일정](docs/09-production-plan.md)** — 2026-04-19 ~ 2026-05-07
- 🎨 **[VARCO 파이프라인](docs/08-varco-pipeline.md)**
- 📦 **[제출물 체크리스트](docs/10-submission.md)**

## Repository Layout

```
/
├── Assets/              # Unity 에셋 루트
│   ├── _Project/        # 본 프로젝트 전용 에셋 네임스페이스
│   │   ├── Art/         # Characters · Environments · Props · Materials · Textures · VFX
│   │   ├── Audio/       # BGM · SFX · Voice · Mixers
│   │   ├── Prefabs/     # Gameplay · Level · UI
│   │   ├── Scenes/      # Boot · MainMenu · Zones
│   │   ├── Scripts/     # Core · Gameplay · Systems · UI
│   │   ├── Settings/    # URP, Input, Audio 설정
│   │   ├── Shaders/
│   │   └── UI/
│   ├── Editor/          # 에디터 확장 스크립트
│   └── ThirdParty/      # 외부 에셋/플러그인 (LFS)
├── docs/                # 모든 기획·설계 문서
│   ├── levels/          # 존별 레벨 디자인 상세
│   └── diagrams/        # 다이어그램 이미지/소스
├── varco_source/        # VARCO 원본 export (3d · sound · voice) — LFS
├── tools/               # 빌드·파이프라인 보조 스크립트
├── build/               # 로컬 빌드 산출물 (gitignored)
├── .gitignore
├── .gitattributes       # Git LFS 구성 포함
└── README.md            # (본 파일)
```

> `ProjectSettings/`, `Packages/`, `Library/`는 Unity 초기 실행 시 자동 생성되며 `Library/`는 `.gitignore`에 포함되어 있습니다.

## Getting Started (개발 환경)

1. **Unity Hub**에서 **2022.3 LTS** 설치 후 본 리포 루트를 프로젝트로 Add
2. URP 템플릿 적용 확인 — `Assets/_Project/Settings/URP_Asset.asset`이 없을 경우 초기 실행 시 생성
3. 첫 씬: `Assets/_Project/Scenes/Boot/Boot.unity` (없다면 Sprint 0에서 생성)
4. Git LFS 설치 필수:
   ```bash
   brew install git-lfs
   git lfs install
   ```

## Team

| 역할 | 담당 |
|---|---|
| Direction / Programming / Level | Jerry (JaerryLee) |
| VARCO 3D · Sound · Voice | Jerry |
| QA | TBD (Sprint 4) |

## License

저작권은 참가자(본 팀)에 귀속됩니다. 공모 요강에 따라 SBA·NC AI는 홍보 목적 범위 내 활용 가능합니다.
본선 진출 및 외부 공개 시 별도 라이선스(CC BY-NC-SA 또는 상용) 결정 예정.

---

*Made with Unity · VARCO 3D · VARCO Sound · VARCO Voice*
