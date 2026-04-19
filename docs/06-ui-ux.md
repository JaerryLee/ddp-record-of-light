# 06 · UI/UX

> 콘트롤 바인딩은 [01 · Game Design §3](01-game-design.md), 자막 처리는 [05 · Audio §7](05-audio-direction.md) 참조.

## 1. Philosophy

> *"UI는 방해물이 아니라 호흡이다."*

이 게임의 UI는 **최대한 디제틱 지향**이다. 플레이어 몰입을 끊지 않도록 대부분의 피드백을 월드 공간으로 보내고, 메뉴/설정 같은 필수 비디제틱 UI만 최소한의 풀스크린 오버레이로 처리한다.

## 2. UI Hierarchy

```
Canvas Stack (렌더 순서 ↓)
 1. System        — Debug overlay (dev only)
 2. Overlay       — Pause, Settings, Title
 3. HUD Non-Diegetic — Subtitles, Interaction prompt, Archive toast
 4. HUD Diegetic    — 프리즘 홀로그램 링, Node 하이라이트
 5. World Space    — 디제틱 패널 (Archive 메뉴, 사인보드)
```

## 3. HUD (In-Game)

### 3.1 기본 원칙
- **아이콘 1개, 텍스트 1줄** 이상 동시에 나오지 않는다
- HUD는 **프리즘 조준 시 페이드-인, 비활성 시 3초 후 페이드-아웃**
- 풀스크린 HUD (헬스바, 미니맵 등) **없음**

### 3.2 요소 목록

| 요소 | 등장 조건 | 위치 | 수명 |
|---|---|---|---|
| Crosshair Dot | 항상 | 화면 중앙 (4×4 px) | 영구 |
| Interaction Prompt | Node/Crystal/사인 시선 내 | Crosshair 아래 | 대상 유지 중 |
| Prism Ghost | Node 조준 시 | 월드 공간 | 대상 유지 중 |
| Subtitle | 보이스 재생 중 | 화면 하단 22% | 보이스 종료 +1s |
| Archive Toast | 새 크리스털 획득 시 | 화면 상단 우측 | 3초 |
| Objective Hint | 3분 정체 시 | 화면 하단 | 5초 |

### 3.3 Interaction Prompt 사양

```
┌──────────────────┐
│  [E]  프리즘 배치 │    ← 한글 버튼/행동 쌍
└──────────────────┘
```

- 배경: 반투명 검정 60%
- 폰트: Pretendard Medium 16pt (한), Inter Medium 16pt (영)
- 아이콘 크기: 24×24
- 영문 "Place Prism" 대체 번역 제공
- 게임패드 모드에서 자동 `[A]` 로 변환 (본선 확장)

## 4. Prism Interaction UX

### 4.1 선택 흐름

```
1. 플레이어가 Node에 시야 정렬 (3 m 이내)
2. 반투명 Prism Ghost 표시 (월드 공간)
3. 좌클릭 → Place (설치음 + Node 고정)
4. Q/E → 15° 회전 (Ghost 상태에서도 가능)
5. 다시 Node 시야 내 좌클릭 → Remove (회수음)
```

### 4.2 시각 피드백
- 배치 가능: Ghost가 **Amber 림**으로 맥동
- 배치 불가 (이미 차있음): Ghost가 **Red 50%**로 표시 + 상호작용 비활성
- 빔 연결 성공: 빔이 **0.3초 걸쳐 glow up** + 크리스털 방향에 빛 번짐

### 4.3 Audio Confirmation
모든 배치·회전·회수는 **0.1~0.2초 SFX** 동반. [05 · Audio §4.1 Prism](05-audio-direction.md) 참조.

## 5. Archive Menu (디제틱)

### 5.1 구조
- `Tab` 키로 토글
- 플레이어 앞 2 m 지점에 **반투명 월드 패널**이 페이드 인 (풀스크린 모달 아님)
- 패널엔 획득한 크리스털만 표시 (미획득은 빈 슬롯 실루엣)

### 5.2 레이아웃

```
┌─────────────────────────────────────┐
│   ARCHIVE — Memories of Seoul       │
│                                     │
│   [C1]  개관 2014         ▶ REPLAY  │
│   [C2]  첫 밤, 첫 불      ▶ REPLAY  │
│   [C3]  런웨이            ▶ REPLAY  │
│   [  ]  ─────                       │
│   [  ]  ─────                       │
│   [  ]  ─────                       │
│                                     │
│   [Tab] 닫기                        │
└─────────────────────────────────────┘
```

### 5.3 리플레이
- 선택 시 해당 크리스털의 Vignette 재생 (플레이어 이동 정지)
- 2번 이상 재생은 허용. 중간 `Esc`로 취소

## 6. Main Menu & Title

### 6.1 Title Screen
- 검은 배경, DDP 외벽 실루엣 1장
- 중앙 하단 메뉴:
  ```
    NEW LIGHT   (신작 시작)
    CONTINUE    (마지막 체크포인트)
    SETTINGS
    CREDITS
    QUIT
  ```
- BGM: `MUS_Intro` 첫 20초 루프

### 6.2 Continue 처리
- 자동 세이브 슬롯 1개만 존재. 체크포인트 기반
- Continue 가능 시점: Zone 1 도입 이후

## 7. Settings

### 7.1 페이지 구성

| 탭 | 옵션 |
|---|---|
| **Display** | Resolution, Window Mode, V-Sync, FPS Cap, Brightness (Gamma) |
| **Graphics** | URP Quality Tier (Low/Mid/High), Motion Blur (Off default), Film Grain (Off default) |
| **Audio** | Master, Music, SFX, Voice, Ambience (0~100) |
| **Subtitles** | On/Off, Size (S/M/L), Speaker Color (On/Off) |
| **Controls** | Mouse Sensitivity, Invert Y, FOV (60~90), Head Bob (On/Off), Hint Voice (On/Off) |
| **Accessibility** | High Contrast Outlines, Colorblind Patterns, Reduce VFX Intensity |

### 7.2 구현
- Unity UIToolkit 또는 UGUI. **1차 제출은 UGUI**로 단순하게
- PlayerPrefs 기반 저장 (서브타이틀·음량·FOV 등)

## 8. Pause

- `Esc`로 진입. 게임 시간 정지 (`Time.timeScale = 0`), 오디오 Pause
- 메뉴: Resume · Settings · Title · Quit
- 배경: 현재 화면 블러 + 페이드 15% 오버레이

## 9. Loading

- Zone 전환 시 보이는 로딩은 **실제 로딩이 아닌 전환 씬**이 담당 (Transition 코리더)
- 예외: 타이틀 → 인트로 진입 시 **검은 페이드 + 아카이브 AI 목소리 선재생**

## 10. Iconography

### 10.1 규칙
- 아이콘 스타일: **단색(Outline) · 1.5 px 선**
- 회전·스케일 애니메이션 허용, 색 반전 비허용
- 키 프롬프트 색: 기본 Cool White. 하이라이트 시 Amber

### 10.2 필수 아이콘 리스트 (1차)
- `icon_interact_E.png`
- `icon_prism_place.png` / `_remove.png` / `_rotate.png`
- `icon_crystal_locked.png` / `_unlocked.png`
- `icon_archive_tab.png`
- `icon_pause.png`
- `icon_subtitle.png`

## 11. Typography

| 용도 | 폰트 | 크기 |
|---|---|---|
| 본문/자막 (한) | Pretendard Medium | 18·22·28 |
| 본문/자막 (영) | Inter Medium | 동일 |
| 제목 (한) | Pretendard Semibold | 36 |
| 넘버·UI 보조 | JetBrains Mono | 14 |

> 모든 폰트는 **OFL 또는 자유 재배포 허용 라이선스**. 배포 폴더에 라이선스 파일 포함.

## 12. Microcopy Tone

- 단호하지 않되 친근하지도 않다. **정중하고 고요**
- 안내 문장은 한 문장 15자 이내. 두 문장 넘지 않음
- 예: `"이 문은 지금 잠겨 있어요."` (O) / `"접근 권한이 없습니다."` (X — 차갑다)

## 13. UX 검증 체크리스트

- [ ] 첫 플레이어가 5초 내에 이동·시점 조작 가능
- [ ] 60초 내에 첫 프리즘 배치 성공
- [ ] 크리스털 활성화 피드백이 "이미 일어난 일"임을 명확히 인지
- [ ] Archive 메뉴 열고 닫기 3초 이내
- [ ] 자막 크기 L에서 화면 가림 ≤ 18%
- [ ] Esc → Settings → Audio → Master 조절까지 4클릭 이내

## Change Log
- **2026-04-19** — 초안
