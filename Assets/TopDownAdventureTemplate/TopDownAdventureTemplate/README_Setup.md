# TopDownAdventureTemplate セットアップメモ

## 1. 必要パッケージ

- Input System
- Cinemachine
- TextMeshPro

## 2. Input Actions

`PlayerInputActions` などの名前で Input Action Asset を作り、Action Map `Player` を用意します。

- `Move`: Action Type = Value / Control Type = Vector2
  - Keyboard: 2D Vector Composite / W A S D
  - Gamepad: Left Stick
- `Attack`: Action Type = Button
  - Mouse: Left Button
  - Gamepad: Button South（PS系なら×相当）
- `Interact`: Action Type = Button
  - Keyboard: F
  - Gamepad: Button West（PS系なら□相当）

作成した各 Action を、Player の `InputReader` に割り当てます。

## 3. Player

<!-- Player GameObject に以下を付けます。

- Rigidbody2D
  - Body Type: Dynamic
  - Gravity Scale: 0
  - Freeze Rotation Z: On
- Collider2D
- InputReader
- PlayerController2D
- PlayerCombat2D
- PlayerHealth2D
- PlayerInventory2D
- PlayerProgression2D
- PlayerInteractor2D

Tag は `Player` にしてください。 -->

## 4. Enemy

<!-- Enemy GameObject に以下を付けます。

- Rigidbody2D
  - Body Type: Dynamic
  - Gravity Scale: 0
  - Freeze Rotation Z: On
- Collider2D
- EnemyController2D
- EnemyHealth2D
- EnemyContactDamage2D

Enemy は `Enemy` レイヤーに置き、PlayerCombat2D の enemyLayer にそのレイヤーを指定します。 -->

## 5. Tilemap

- GroundTilemap: 歩ける床。基本コライダー無し。
- ObstacleTilemap: 壁、木、岩、水など。TilemapCollider2D + CompositeCollider2D 推奨。
- DecorationTilemap: 当たり判定なし装飾。

ObstacleTilemap の Rigidbody2D は Static にします。

## 6. 宝箱・鍵・扉

- KeyPickup2D: Trigger Collider2D を付ける。Playerが触れると鍵数が増える。
- TreasureChest2D: Collider2D と TreasureChest2D を付け、Interactable 用レイヤーに置く。
- LockedDoor2D: Collider2D と LockedDoor2D を付ける。鍵があれば接触時に開く。

## 7. UI

<!--
Canvas に Slider と TextMeshProUGUI を作り、HUDController2D に割り当てます。

表示例：

- HPバー
- 鍵 × 1
- Coin × 10
- 攻撃力 15
- 撃破 10 -->

## 8. Camera

Cinemachine Camera を作り、Follow に Player を設定します。
マップ外を映したくない場合は、Cinemachine Confiner 2D 用の PolygonCollider2D を作って範囲を指定します。

## 9. 注意

このテンプレートの敵追跡は、初心者が読みやすいように「プレイヤー方向へ直進する」方式です。
壁を回り込む本格的な最短経路探索が必要な場合は、A\* Pathfinding などを別途追加してください。
