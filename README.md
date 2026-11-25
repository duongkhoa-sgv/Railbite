# Railbite

Ứng dụng đặt món ăn trên tàu: hành khách quét mã toa/ticket, chọn món, thanh toán bằng ví nội bộ; nhà hàng duyệt đơn và phân công giao; nhân viên giao cập nhật trạng thái; hành khách theo dõi đơn bằng polling.

## Công nghệ

| Thành phần | Công nghệ |
|---|---|
| Backend | ASP.NET Core `net10.0`, EF Core `10.0.15`, SQL Server (`.\SQLEXPRESS`) |
| Auth | JWT access token (12h), BCrypt |
| API docs | OpenAPI tại `/openapi/v1.json` (Development) |
| Mobile | Flutter 3.50 (`material_ui`), Dart 3.14, dio, go_router, provider |
| Diagrams | PlantUML trong `docs/diagrams` |

## Cấu trúc

```
Railbite.slnx
src/
  Railbite.Domain/          Entities + Enums
  Railbite.Application/     DTOs, interfaces, services (BCrypt, JWT)
  Railbite.Infrastructure/  EF Core: AppDbContext, Configurations, Migrations, DbSeeder
  Railbite.Api/             Controllers, Program.cs, appsettings
app/                        Flutter app (railbite_app)
docs/diagrams/              5 sơ đồ PlantUML
```

## Yêu cầu môi trường

- .NET SDK 10
- SQL Server Express (instance `.\SQLEXPRESS`)
- Flutter 3.50+ và Android SDK/emulator (khuyến nghị AVD `Pixel_API36`)
- Git + Git Credential Manager (để push GitHub)

## Chạy Backend

1. Khôi phục package và công cụ:

   ```powershell
   dotnet tool restore
   dotnet restore
   ```

2. Cấu hình JWT key bằng **user-secrets** (không commit key):

   ```powershell
   dotnet user-secrets set "Jwt:Key" "<chuoi-bi-mat-toi-thieu-32-ky-tu>" --project src/Railbite.Api
   ```

   > Mỗi thành viên tự đặt key riêng. `Jwt:Key` **không** nằm trong `appsettings.json`.

3. Áp dụng migration (DB `Railbite` được tạo tự động):

   ```powershell
   dotnet ef database update --project src/Railbite.Infrastructure --startup-project src/Railbite.Api
   ```

4. Chạy API (http://0.0.0.0:5000):

   ```powershell
   dotnet run --project src/Railbite.Api
   ```

   Kiểm tra: `GET http://localhost:5000/health` → `200`.

### Tài khoản seed (mật khẩu `Passw0rd!`)

| Vai trò | Email |
|---|---|
| Passenger | `passenger@railbite.local` |
| Restaurant | `restaurant@railbite.local` |
| DeliveryPerson | `delivery@railbite.local` |

## Chạy Flutter

```powershell
cd app
flutter pub get
flutter run
```

- Emulator Android gọi API qua `http://10.0.2.2:5000/api/v1` (đã cấu hình trong `lib/core/config/app_config.dart`).
- Thiết bị thật: đổi base URL sang IP LAN của máy chạy API.

## Quy ước Git

- `main`: baseline ổn định, chỉ merge khi feature build/test OK.
- Mỗi tính năng tạo nhánh riêng: **`feat/<ten-tinh-nang>`**, ví dụ:
  - `feat/auth-api`
  - `feat/food-api`
  - `feat/ticket-order`
  - `feat/flutter-auth`
- Commit nhỏ, message rõ ràng; merge `--no-ff` về `main` (hoặc mở Pull Request trên GitHub).
- Không commit `bin/`, `obj/`, `.dart_tool/`, `build/`, hay bất kỳ secret nào.

## Sơ đồ

Xem `docs/diagrams`:

- `architecture.puml`
- `class.puml`
- `usecase-passenger.puml`
- `usecase-restaurant.puml`
- `usecase-deliveryperson.puml`
