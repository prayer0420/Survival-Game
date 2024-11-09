# SprataSurvival 

## 🎤프로젝트 소개
🏠건축하고! 🌳채집하고! 🐴사냥해서 살아남자!! 초보 모험가의 생존 서바이벌!! <br>
<img width = 400px src = "https://github.com/user-attachments/assets/ea68b16c-fa59-4e33-936c-c04652b78f36"><br>

## 👨‍👨‍👦 팀 구성 및 역할 소개
| 역할   | 이름     | 담당 업무 |
|--------|----------|-----------------------------------------|
| 팀장   | 이승환   | 플레이어 화면, 이동, 생존 관리          |
| 팀원   | 서보훈   | 인벤토리, 아이템                        |
| 팀원   | 안성찬   | AI (기본 이동, 경계, 추적, 공격, 도망, 죽음 상태), 적 생성 |
| 팀원   | 박기도   | 건축, 데이터 저장, 오디오 시스템        |

## 개발기간
2024.10.31 ~ 2024.11.07

## 주요기능
> ## 플레이어 상태 및 입력
> * 플레이어의 다양한 상태 구현 : 체력, 배고픔, 스테미나, 갈증 <br>
> <img width = 300px src = "https://github.com/user-attachments/assets/2a101146-d4be-425b-b4f5-17f8436391f8"><br>

> ### 특수환경 요소 추가
> * 높은 온도의 지역에서는 플레이어의 체력이 감소합니다<br>
> * 물이 있는 지역에서는 플레이어의 이동속도 감소합니다<br>

> ### InputSystem 활용
> * InputSystem으로 각종 키, 마우스의 입력을 받습니다<br>
> * 방향키 조작 : WASD,
> * 아이템 줍기: E, 인벤토리창: Tab, 건축시스템 : B, 일시정지 및 메뉴: P
> <img width = 350px src = "https://github.com/user-attachments/assets/0d526a1e-b1be-429d-82ed-20aaf30a0e18">
> <img width = 500px src = "https://github.com/user-attachments/assets/ddc6afb1-3da3-48f5-a94e-528853fe5868"><br>
> 

> ## 사냥 시스템
> ### State패턴을 적용한 적 AI구현
> <img width = 500px src = "https://github.com/user-attachments/assets/1b4acb68-92ce-4ccb-824a-6fb20503c3b9"><br>

> **Idle 및 Wander 상태**<br>
> <img width = 300px src = "https://github.com/user-attachments/assets/88a4c641-dbc8-4562-bdce-cccb33ed081b"><br>

> **Alerted 상태**<br>
> <img width = 300px src = "https://github.com/user-attachments/assets/d5d76725-ba30-4677-ae32-b5d7b014b1cc"><br>

> **Chase 및 Attack 상태**<br>
> <img width = 300px src = "https://github.com/user-attachments/assets/8d992758-0f25-4a40-8ebb-42dea6fda10f"><br>

> **Esacape상태**<br>
> <img width = 300px src = "https://github.com/user-attachments/assets/3c604567-c874-4166-bfbd-fb077f782e9e"><br>

> ### 사냥을 통해 각종 아이템을 획득
> **Dead상태** <br>
> <img width = 300px src = "https://github.com/user-attachments/assets/5c3e845d-bc71-44e6-93a8-c145e418d13a"><br>

> ## Scriptable을 사용한 아이템 구현 및 관리
> <img width = 500px src = "https://github.com/user-attachments/assets/e50d8cdc-c1d5-4e71-9222-21327f172117"><br>

> ### 인벤토리 및 장착 시스템
> * 필드에서 아이템을 획득하면 인벤토리에 추가합니다.<br>
> * 아이템의 종류에 따라 아이템 설명 및 아이템 사용시 기능을 상이하게 구현했습니다(전략패턴)<br>
> <img width = 300px src = "https://github.com/user-attachments/assets/c536e0df-f7bf-44d0-9fae-73e231c99f5c"><br>

> * 무기 또는 도구 아이템을 장착할 수 있습니다.<br>
> <img width = 300px src = "https://github.com/user-attachments/assets/c038df08-24d3-4389-8a75-9ed3a5eff1f8"><br>
> <img width = 300px src = "https://github.com/user-attachments/assets/1d9540df-28c6-459e-b459-5882248babed"><br>

> ## 제작 시스템
> * 재료를 모아 원하는 아이템(무기,건축물 등)을 제작할 수 있습니다!<br>
> <img width = 300px src = "https://github.com/user-attachments/assets/282dd5ab-f4a8-419b-9fa2-4769847459ef"><br>
> <img width = 600px src = "https://github.com/user-attachments/assets/73b11178-f165-4711-9c8b-a30e2f7d0e0f"><br>

> ## 건축 시스템
> * 제작한 건축물을 맵에서 건축(수정,파괴)할 수 있습니다!<br>
> <img width = 300px src = "https://github.com/user-attachments/assets/7d0cda47-df55-4b21-b41e-892ee425dc0d"><br>
> <img width = 600px src = "https://github.com/user-attachments/assets/a81d6959-ac1b-4543-a253-02765f9da738"><br>

> ## 세이브 및 오디오 시스템
> * 데이터를 저장 및 불러올 수 있습니다.
> * 오디오의 설정을 변경할 수 있습니다.



## 기타
* Version: Unity 2022.03.17f
* Asset: Survival Engine - Crafting, Building,Farming (Indie Marc)
