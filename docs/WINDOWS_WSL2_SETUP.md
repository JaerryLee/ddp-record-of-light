# Windows + WSL2 에서 작업 이어받기

> ** Windows PC에 ** 환경을 세팅하고, `main` 브랜치의 최신 상태(`db9817e` 기준)에서 Sprint 1 그레이박스 작업을 이어받는 **가이드**.
>
> 전제: 이 리포의 최신 `main`은 GitHub(`JaerryLee/ddp-record-of-light`)에 푸시돼 있고, 맥 쪽에는 uncommitted 변경 없음.

---


**환경 셋업(§2~§5)이 끝나면 바로 [§6 Sprint 1 Day 2](#6-이어서-할-작업--sprint-1-day-2)** 로 직행해서 다음 작업을 끝내야 합니다. 맥 세션에서 **아직 안 한 미완료** 항목입니다:

| # | 작업 | 결과물 |
|---|---|---|
| 1 | `Assets/_Project/Scenes/Zones/Zone1_Alimter.unity` 신규 씬 저장 | 씬 파일 |
| 2 | 기본 씬 정리 (Main Camera 삭제, Directional Light 유지) | — |
| 3 | `Ground` Cube 생성 (Position 0,-0.5,0 / Scale 30,1,30) | Hierarchy |
| 4 | **`Player` 오브젝트 + `CameraRoot` + `Camera` 트리** + `CharacterController` + `PlayerController` 컴포넌트 | `Assets/_Project/Prefabs/Gameplay/Player.prefab` |
| 5 | `_Systems` 오브젝트 + `BeamResolver` + `VignettePlayer` + `PrismInventory` | Hierarchy |
| 6 | Layer 6 `Interactable` / 7 `Beam` / 8 `Prism` 등록 | TagManager |
| 7 | ▶ Play → WASD 이동 + 마우스 시선 동작 확인 | Sprint 1 Day 2 DoD ✅ |
| 8 | Ctrl+S → `git commit` (push는 본인) | 커밋 1건 |

상세 절차는 §6.1~§6.8. 환경 준비 끝나면 **다른 작업 전에 이것부터**.

---

## 0. 건너뛰어도 되는 맥락 (이미 된 것)

| 완료 항목 | 커밋 |
|---|---|
| Unity 6 (`6000.4.3f1`) 프로젝트 스켈레톤 | `39a6cf9` |
| Sprint 1 코어 스크립트 9종 (PlayerController, Prism, Beam, Crystal, Vignette) | `39a6cf9` |
| collab-proxy 제거 (CS0104 해결) | `1dbe017` |
| URP 기본 에셋 자동 생성 | `487e027` |
| GitHub 이슈 템플릿·라벨 | `29277db` |
| 크로스플랫폼 문서 정비 (Unity 6 + macOS/Windows) | `796add9` |

남은 작업 = Sprint 1 Day 2 이후: **첫 씬(`Zone1_Alimter.unity`) 화이트박스**와 **Player 프리팹 구성** 부터.

---

## 1. Quickstart (15분 코스)

이미 Windows에 Git · Unity Hub · Unity 6 LTS · WSL2 Ubuntu가 있다면 아래 5줄만:

```powershell
# PowerShell
cd C:\Dev
git clone https://github.com/JaerryLee/ddp-record-of-light.git
cd ddp-record-of-light
git lfs install
# Unity Hub → Open → C:\Dev\ddp-record-of-light
```

없으면 아래 §2~§5 따라 설치.

---

## 2. Windows 호스트 측 필수 설치

> **Unity Editor는 Windows 네이티브**로 돌아갑니다. WSL2 안에서는 Unity를 돌리지 않습니다 (Linux Editor는 별개 빌드이고 이 프로젝트 대상 아님).

### 2.1 Git for Windows
- <https://git-scm.com/download/win>
- 설치 시 옵션 권장:
  - *Git from the command line and also from 3rd-party software*
  - *Checkout as-is, commit Unix-style line endings* (또는 **Checkout as-is, commit as-is** — .gitattributes가 `eol=lf` 강제하므로 둘 다 OK)
  - *Use Git Credential Manager Core*
- **Git LFS**는 최신 Git for Windows에 기본 포함

### 2.2 Unity Hub + Unity 6 LTS
1. <https://unity.com/download> → Unity Hub (Windows) 설치
2. Hub 실행 → Sign in (Unity 계정)
3. Hub → **Installs → Install Editor** → `6000.4.3f1` (LTS) 선택
4. **꼭 체크할 모듈**:
   - Microsoft Visual Studio Community 2022 (없으면 따로 설치해도 됨)
   - **Windows Build Support (IL2CPP)** — 빌드 타겟
   - **Mac Build Support (Mono)** — 선택, 크로스 빌드 연습용
   - Documentation — 선택
5. 설치 용량 약 10~15 GB. 10~20분 소요

### 2.3 코드 에디터 (택일)
- **JetBrains Rider** (권장, Unity 통합 최강) — 유료
- **Visual Studio 2022 Community** — 무료, Unity Hub에서 같이 설치
- **VS Code** + C# Dev Kit 확장 — 무료, 가볍지만 디버거 설정 별도

### 2.4 GitHub CLI (선택)
- <https://cli.github.com/> → Windows msi 설치
- `gh auth login` → HTTPS → 브라우저 인증

---

## 3. WSL2 Ubuntu 세팅

> Git 커맨드·Claude Code·shell 작업은 WSL2가 맥에서 쓰던 zsh 환경에 가장 가깝습니다.

### 3.1 WSL2 활성화
관리자 PowerShell에서:
```powershell
wsl --install -d Ubuntu-24.04
# 재부팅 후 Ubuntu 터미널이 자동 실행 → username / password 설정
```

### 3.2 Ubuntu 내부 도구 설치
```bash
sudo apt update
sudo apt install -y git git-lfs curl unzip zsh build-essential
git lfs install

# GitHub CLI (apt 저장소 등록)
curl -fsSL https://cli.github.com/packages/githubcli-archive-keyring.gpg \
  | sudo dd of=/usr/share/keyrings/githubcli-archive-keyring.gpg
sudo chmod go+r /usr/share/keyrings/githubcli-archive-keyring.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/githubcli-archive-keyring.gpg] https://cli.github.com/packages stable main" \
  | sudo tee /etc/apt/sources.list.d/github-cli.list
sudo apt update && sudo apt install -y gh
gh auth login   # JaerryLee 계정
```

### 3.3 Git identity (맥과 동일하게)
```bash
git config --global user.email "jerrylee1516@gmail.com"
git config --global user.name "JaerryLee"
git config --global init.defaultBranch main
git config --global pull.rebase false
```

### 3.4 (선택) Claude Code
맥에서 쓰던 Claude Code 정책(`~/.claude/CLAUDE.md` — "never `git push` 자동, Co-Authored-By 금지") 을 WSL2에도 유지하려면:
- Claude Code를 WSL2 Ubuntu 안에 설치 (<https://docs.claude.com/claude-code/install>)
- 맥의 `~/.claude/CLAUDE.md`·`~/.claude/settings.json` 을 복사
  ```bash
  # 맥에서: scp / 클립보드로 옮기거나, dotfiles 리포 사용
  # WSL2에서 직접 작성도 가능 (이 가이드에 스냅샷 §3.4 참조)
  ```
- 내용은 다음과 같아야 합니다 (핵심):
  - `attribution.commit = ""` / `pr = ""`
  - `permissions.deny = ["Bash(git push)", "Bash(git push *)"]`
  - CLAUDE.md: push는 사용자 수동, Claude 코트레일러 금지

---

## 4. 프로젝트 클론 — **어디에 둘지가 성능을 갈라놓습니다**

### 4.1 배치 원칙

| 위치 | Unity 속도 | Git/shell 속도 |
|---|---|---|
| **Windows 파일시스템 (`C:\...`)** ✅ 권장 | 빠름 (네이티브) | WSL에서 `/mnt/c/...` 접근 시 약간 느림 |
| WSL2 파일시스템 (`~/...`) | **매우 느림** (9P 프로토콜 + inotify 대량) | 빠름 |

**결론**: 프로젝트 자체는 Windows 측 `C:\Dev\ddp-record-of-light` 같은 경로에 두고, WSL에서 `/mnt/c/Dev/ddp-record-of-light` 로 접근.

Unity 프로젝트가 WSL 쪽에 있으면 import가 30분+ 걸리고 파일 감지가 깨지는 경우도 있습니다. 무조건 Windows 측 경로.

### 4.2 클론

PowerShell 또는 Git Bash (Windows) 또는 WSL2 `/mnt/c/...` 안에서:

```bash
cd /mnt/c/Dev   # 또는 PowerShell에서 cd C:\Dev
mkdir -p Dev && cd Dev
git clone https://github.com/JaerryLee/ddp-record-of-light.git
cd ddp-record-of-light
git lfs install
git lfs pull   # LFS 파일 있는 경우 (지금은 없지만 예방적)
```

### 4.3 Windows Defender 예외 (성능 필수)

Library/, Temp/ 에 Defender가 실시간 검사하면 Unity가 5배 느려집니다.

PowerShell 관리자:
```powershell
Add-MpPreference -ExclusionPath "C:\Dev\ddp-record-of-light"
Add-MpPreference -ExclusionProcess "Unity.exe"
Add-MpPreference -ExclusionProcess "UnityHub.exe"
```

---

## 5. Unity 에서 프로젝트 열기

1. Unity Hub → **Open** → `C:\Dev\ddp-record-of-light` 선택
2. 에디터 버전 경고가 뜨면 `6000.4.3f1` 확인하고 Continue
3. 최초 import 5~15분 소요
   - URP, Input System, TextMeshPro 등 패키지 재해결
   - collab-proxy는 `manifest.json`에서 이미 제거됨 → **CS0104 에러 없음**
4. 첫 실행 시 **"This project is using the new input system package but the native platform backends..." Warning** 뜨면 **Yes** → 에디터 재시작 → 입력 백엔드 활성화
5. Console 0 error / 0 warning (yellow 경고바는 Input Manager deprecation — 무시 가능)

> 맥에서 겪은 Safe Mode 시퀀스는 collab-proxy 제거가 리포에 반영돼 있어서 **Windows에서는 다시 발생하지 않습니다**.

---

## 6. 이어서 할 작업 — Sprint 1 Day 2

현재 달성 상태:
- [x] Sprint 1 Day 1: 코어 스크립트 스캐폴드 (`PlayerController`, `PrismNode`, `BeamResolver`, `MemoryCrystal`, `VignettePlayer`)
- [ ] **Sprint 1 Day 2**: 첫 씬 + Player 프리팹 + Systems 오브젝트 배치 ← 여기부터

[docs/UNITY_SETUP.md](UNITY_SETUP.md) §4~§7 또는 아래 축약 버전 그대로:

### 6.1 첫 씬 저장
- File → Save As → `Assets/_Project/Scenes/Zones/Zone1_Alimter.unity`

### 6.2 기본 정리
- Hierarchy에서 `Main Camera` 삭제 (Player가 자체 카메라)
- `Directional Light` 유지

### 6.3 Ground
- 우클릭 → 3D Object → Cube
- Transform: Position `(0, -0.5, 0)`, Scale `(30, 1, 30)`
- Rename: `Ground`

### 6.4 Player 프리팹
```
Player (Empty, Position 0,0,0)
├─ Character Controller 컴포넌트  (Height 1.8, Radius 0.3, Center (0, 0.9, 0))
├─ Player Controller 컴포넌트     (우리 스크립트)
└─ CameraRoot (Empty, Position 0, 1.6, 0)
   └─ Camera (GameObject → Camera, FOV 70)
```
- Player 선택 → Inspector의 `Player Controller` → `Camera Root` 필드에 `CameraRoot` 드래그
- `Assets/_Project/Prefabs/Gameplay/` 에 Player 드래그하여 Prefab화

### 6.5 Systems 오브젝트
```
_Systems (Empty)
├─ Beam Resolver
├─ Vignette Player
└─ Prism Inventory
```

### 6.6 Layer 생성
Edit → Project Settings → Tags and Layers → User Layers:
- 6: `Interactable`
- 7: `Beam`
- 8: `Prism`

### 6.7 플레이 테스트 ✅
- ▶ 누르고 WASD 이동 + 마우스 시선 동작 확인
- Escape로 정지, Cmd+S (Windows는 Ctrl+S) 씬 저장

### 6.8 커밋 & 푸시
```bash
cd /mnt/c/Dev/ddp-record-of-light
git add Assets/_Project/Scenes/ Assets/_Project/Prefabs/
git status --short    # 씬 .unity, 프리팹 .prefab, 각 .meta 확인
git commit -m "feat(sprint1): Zone1 whitebox + Player prefab + Systems"
# push는 본인이 실행 (Claude Code 자동 금지 정책 유지):
git push origin main
```

---

## 7. 크로스플랫폼 주의사항

### 7.1 줄바꿈 (Line Endings)

`.gitattributes`에 `* text=auto eol=lf` 선언돼 있어 **모든 텍스트가 LF**로 커밋됩니다. Windows 측 워킹 트리에서 에디터가 CRLF로 보여도 커밋 시 LF로 변환되므로 걱정 없음.

혹시 첫 체크아웃에서 수백 건 "modified" 오탐이 뜨면:
```bash
git config core.autocrlf false
git rm --cached -r .
git reset --hard
```

### 7.2 경로 길이 제한

Windows NTFS 기본 260자 상한. Unity 프로젝트가 깊어지면 터질 수 있음:
```powershell
# 관리자 PowerShell
New-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Control\FileSystem" `
  -Name "LongPathsEnabled" -Value 1 -PropertyType DWORD -Force
```
그리고 git:
```bash
git config --system core.longpaths true
```

### 7.3 WSL2 리소스 상한 (필요 시)

`C:\Users\<user>\.wslconfig`:
```ini
[wsl2]
memory=8GB
processors=6
swap=4GB
```
WSL2 재시작: `wsl --shutdown` 후 다시 ubuntu.

### 7.4 `_Project` 폴더 이름의 언더바

Unity는 `Assets/_Project/` 네임스페이스를 정렬 최상단에 띄우기 위해 관습적으로 `_` 접두를 씁니다. Windows에서도 정상 동작.

---

## 8. 커밋·푸시 정책 (맥에서 이미 정한 규칙 그대로)

- **Claude Code는 `git push`를 자동 실행하지 않음** (`~/.claude/settings.json`의 `permissions.deny` + `~/.claude/CLAUDE.md`)
- **커밋 메시지에 `Co-Authored-By: Claude ...` 트레일러 안 들어감** (`attribution.commit = ""`)
- 푸시는 항상 사용자 수동. WSL에서도 동일하게 유지하고 싶으면 §3.4 참조해 설정 복사

---

## 9. 트러블슈팅

| 증상 | 해결 |
|---|---|
| Unity가 Safe Mode로 들어감 + CS0104 ObjectInfo 에러 | 있어서는 안 됨. 있으면 `Library/PackageCache/com.unity.collab-proxy*` 삭제 후 재오픈 |
| Unity import가 10분 이상 멈춤 | Defender 예외 등록(§4.3) 또는 프로젝트가 WSL 측에 있는지 확인 (§4.1) |
| `git push` 권한 거부 | `gh auth login` 다시 또는 PAT 갱신 (<https://github.com/settings/tokens>) |
| C# 스크립트가 Rider/VS에서 인식 안 됨 | Unity에서 Edit → Preferences → External Tools → External Script Editor 재선택 → Regenerate project files |
| LFS 파일이 포인터로만 보임 | `git lfs install && git lfs pull` |
| WSL에서 `/mnt/c/...` 파일이 읽기 전용 | `sudo umount /mnt/c && sudo mount -t drvfs C: /mnt/c -o metadata,uid=1000,gid=1000` |

---

## 10. 다음 마일스톤 체크리스트 (Sprint 1 완료 조건)

- [ ] `Zone1_Alimter.unity` 씬이 리포에 커밋됨
- [ ] `Player.prefab` 커밋됨
- [ ] 플레이 모드에서 WASD · 마우스 · E 상호작용 동작
- [ ] Console 에러 0 / 경고 최소 (Input Manager deprecation 은 허용)
- [ ] Sprint 2 착수 준비: Splitter/Filter 프리즘 구현 ([docs/01-game-design.md §4.2](01-game-design.md))

Sprint 1 완료되면 [docs/09-production-plan.md](09-production-plan.md) Sprint 2로 이어서.

---

## 11. 유사시 롤백

만약 Windows 환경에서 무언가 꼬이면, 맥에서 하던 시점으로 리셋:
```bash
git fetch origin
git reset --hard db9817e    # 본 가이드 작성 시점의 커밋
```
이 커밋은 Sprint 1 Day 1까지의 모든 작업 포함.

---

*마지막 업데이트: 2026-04-19. 맥(`/Users/jerry/Games`)에서 Sprint 1 Day 1 완료 직후 작성.*
