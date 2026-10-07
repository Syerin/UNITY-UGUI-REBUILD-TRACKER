# Changelog

## [1.2.0] - 2026-10-08

### 추가
- 원인 후보(`CAUSE`) 표시: 같은 프레임에 레이아웃 루트 안에서 Graphic 리빌드된 요소에 Hierarchy 배지 `CAUSE`, Game 뷰 라벨 `CAUSE ·`, 하늘색 외곽선.
- 씬에 트래커가 둘 이상이면 콘솔에 경고를 남긴다. 같은 리빌드를 겹쳐 그리기 때문이다.
- README에 AI 사용, `LAYOUT` 과 `CAUSE` 의 뜻, 버전을 고정한 설치 URL, 새 GIF(UNITY-UI-MVP 임무 화면 녹화).

### 고침
- Play 모드에서 아무것도 표시되지 않던 문제. uGUI 2.6.0(Unity 6000.6)은 에디터에서 Play에 들어갈 때 `CanvasUpdateRegistry` 를 새로 만드는데(`ResetStaticsOnLoad`), 켜질 때 한 번 찾아 둔 옛 레지스트리의 큐를 계속 읽고 있었다. 읽을 때마다 `CanvasUpdateRegistry.instance` 와 비교해 바뀌었으면 큐를 다시 찾는다.
- 에디터에서 스크립트를 다시 컴파일한 뒤(도메인 리로드) 매 프레임 `NullReferenceException` 이 나던 문제(1.1.0). 준비 여부를 큐 참조로만 판단한다.
- 화면을 닫은 프레임에 `MissingReferenceException` 이 나던 문제. 큐에 남은 파괴된 항목(루트가 파괴된 `LayoutRebuilder` 등)을 uGUI처럼 `IsDestroyed()` 로 거른다.
- ScrollRect가 켜지는 프레임처럼 `Canvas.ForceUpdateCanvases()` 가 큐를 먼저 처리하면 그 프레임의 리빌드가 보이지 않던 문제(1.1.0). 큐를 `LateUpdate`(`DefaultExecutionOrder(32000)`)가 아니라 처리 직전인 `Canvas.preWillRenderCanvases` 에서 읽는다.
- 레이아웃 큐의 항목이 `LayoutRebuilder` 로 표시되던 문제. 루트에 붙은 레이아웃 컴포넌트 이름(LayoutGroup · ContentSizeFitter 등)으로 보여 준다.
- 60프레임 창을 벗어나 흐려지는 요소가 `(0/60f)` 로 남던 라벨. 이때는 이름만 둔다.
- README의 "원인 쪽(`LAYOUT` 배지)을 보면 됩니다" 설명. `LAYOUT` 은 원인이 아니라 다시 계산된 레이아웃 루트다.

### 바뀜
- 큐(`IndexedSet`)를 `IList<ICanvasElement>` 로 읽어 매 프레임 리플렉션(`FieldInfo.GetValue`)을 없앴다. 활성 요소 수(`Count`)만 센다.

### 확인
- Unity 6000.6(uGUI 2.6.0)에서 확인했다. Play에서 화면을 여는 프레임(ScrollRect의 `ForceUpdateCanvases`)과 탭 전환(`VerticalLayoutGroup` 에 `LAYOUT`, 셀 텍스트에 `CAUSE`)이 잡히고, 화면을 닫을 때와 에디터에서 스크립트를 다시 컴파일한 뒤에도 콘솔 오류가 없다.

## [1.1.0] - 2026-10-06

### 추가
- UPM 패키지 구성(`package.json`, asmdef). git URL로 설치할 수 있다.
- Unity 6.6 이전 버전용 레거시 Hierarchy 경로(instanceID 기반)를 버전 define으로 나눠 넣었다.
- 한 요소가 Graphic · Layout 큐에 모두 잡히면 두 종류를 함께 표시한다(`LAYOUT+G`).
- uGUI 내부 필드를 찾지 못하면 콘솔에 경고를 한 번 남긴다(전에는 조용히 아무것도 그리지 않았다).
- MIT 라이선스 파일.

### 고침
- 같은 프레임에 두 큐에 모두 있는 요소를 두 번 세던 문제. 마지막 기록 대신 가장 오래된 기록과 비교하고 있었다.
- 집계 창이 61프레임이던 것을 60프레임으로 맞췄다.
- `playModeStateChanged` 를 OnEnable에서 구독하고 OnDestroy에서 해제해, 컴포넌트를 껐다 켜면 중복 구독되던 문제.
- README가 설명하는 새 Hierarchy 지원과 `Highlight Fade Speed` 가 이 저장소의 코드에 빠져 있던 문제.

### 바뀜
- 실행 순서를 가장 늦게(`DefaultExecutionOrder(32000)`) 두어, 다른 스크립트의 LateUpdate가 만든 리빌드도 잡는다.
- 리플렉션은 켜질 때 한 번만 한다. OnGUI는 Repaint 이벤트에서만 그리고, Canvas와 라벨 문자열을 캐싱해 도구 자체의 GC를 줄였다.
- 릴리스 빌드에서는 스스로 꺼진다(`Debug.isDebugBuild`).
- 인스펙터 이름 `Decay Speed` → `Highlight Fade Speed`. 기존 값은 `FormerlySerializedAs` 로 이어진다.
- 네임스페이스 `Syerin.UIRebuild`.

## [1.0.0] - 2026-10-06

- 첫 공개.
