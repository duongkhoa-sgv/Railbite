import 'package:material_ui/material_ui.dart';
import 'package:provider/provider.dart';

import '../../auth/state/auth_provider.dart';

class RoleHomePage extends StatelessWidget {
  const RoleHomePage({super.key, required this.role});

  final String role;

  @override
  Widget build(BuildContext context) {
    final auth = context.watch<AuthProvider>();

    return Scaffold(
      appBar: AppBar(
        title: Text('Railbite — $role'),
        actions: [
          IconButton(
            tooltip: 'Đăng xuất',
            onPressed: () async {
              await auth.logout();
            },
            icon: const Icon(Icons.logout),
          ),
        ],
      ),
      body: Center(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            const Icon(Icons.train, size: 72),
            const SizedBox(height: 12),
            Text('Xin chào, ${auth.user?.name ?? ''}'),
            const SizedBox(height: 8),
            Text('Vai trò: ${auth.user?.role ?? role}'),
          ],
        ),
      ),
    );
  }
}
