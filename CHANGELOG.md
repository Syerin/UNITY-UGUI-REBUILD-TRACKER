# Changelog

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
