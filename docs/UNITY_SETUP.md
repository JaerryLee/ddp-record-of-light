# Unity 6 최초 오픈 셋업

> 이 문서는 본 리포를 처음 Unity Hub에 여는 사람을 위한 **원샷 셋업 가이드**. 5~10분 소요.

## 1. Unity Hub에서 프로젝트 열기

1. Unity Hub → **Open** (New Project 아님!)
2. 본 리포 루트 (`/.../ddp-record-of-light` 또는 현재 `/Users/jerry/Games`) 선택
3. 버전 경고가 뜨면 `6000.4.3f1` 설치 후 재시도. 다른 마이너 버전도 일반적으로 호환되지만 LTS 공유를 권장
4. 최초 오픈 시 패키지 해결 중 수 분 소요 (URP 17 · Input System 1.11 · TextMeshPro 등)

## 2. Input System 활성화

Project Settings → **Player → Other Settings → Active Input Handling**
→ `Input System Package (New)` 선택 → 재시작 묻는 팝업 OK.

> 이 설정 없이는 `PlayerController.cs`의 `Keyboard.current` / `Mouse.current` 가 null 상태로 남아 입력이 먹지 않습니다.

## 3. URP Asset·Renderer 확인 (자동)

**Unity 6은 첫 실행 시 URP 기본 에셋을 자동 생성합니다.** 리포를 Open 후 `Assets/` 루트에 다음 파일이 있으면 셋업 완료:

- `UniversalRenderPipelineGlobalSettings.asset`
- `DefaultVolumeProfile.asset`

없으면 수동 생성:
1. `Assets/_Project/Settings/` 에서 우클릭 → **Create → Rendering → URP Asset (with Universal Renderer)**
2. 이름: `URP_Asset`
3. Project Settings → **Graphics → Scriptable Render Pipeline Settings** 에 드래그
4. Project Settings → **Quality** 각 티어 (Low/Mid/High)에도 할당

## 4. 최초 씬 생성

```
File → New Scene → Basic (URP)
→ Save As: Assets/_Project/Scenes/Zones/Zone1_Alimter.unity
```

최소 구성:
- `Main Camera` 는 비활성화 또는 제거 (PlayerController의 카메라를 사용)
- `Directional Light` 1개
- 바닥 Cube (스케일 20×1×20)
- 빈 GameObject `_Systems` 생성 후:
  - `BeamResolver` 컴포넌트 추가 (빔 머티리얼은 `Assets/_Project/Art/Materials/` 에 Unlit Additive Material 하나 만들어 할당)
  - `VignettePlayer` 컴포넌트 추가
  - `PrismInventory` 추가 (프리즘 프리팹 연결은 Sprint 2)
- 플레이어 프리팹 수동 구성 (아래 §5)

## 5. Player 프리팹 구성

1. 빈 GameObject `Player` 생성 → `CharacterController` 추가
   - Height 1.8, Radius 0.3, Center (0, 0.9, 0)
2. 자식 GameObject `CameraRoot` 추가 → 그 아래 `Camera`
   - CameraRoot localPosition (0, 1.6, 0)
   - Camera FOV 70
3. `Player` 루트에 `PlayerController` 컴포넌트 추가
   - CameraRoot 필드에 `CameraRoot` 드래그
4. Prefab화: `Assets/_Project/Prefabs/Gameplay/Player.prefab`

## 6. Layer 세팅

Tags and Layers → User Layers:
- Layer 6: `Interactable`
- Layer 7: `Beam` (빔 자체가 다른 빔과 간섭하지 않도록, LineRenderer 콜라이더 없으므로 실제로는 선택)
- Layer 8: `Prism`

`PlayerController.interactMask` 는 `Interactable`만 포함.
`BeamResolver.beamMask` 는 Default + Interactable + Prism 포함, Beam 제외.

## 7. 플레이 테스트

씬 재생 → 콘솔 에러 0건 → WASD 이동 / 마우스 시선 / E 상호작용 동작 확인.

---

설치된 다른 LTS 마이너 버전으로 열어도 대부분 동작하지만, 리포 머지 분쟁 방지를 위해 팀 전원 `6000.4.3f1` 고정을 권장합니다.
