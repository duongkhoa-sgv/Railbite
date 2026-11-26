import 'package:go_router/go_router.dart';
import 'package:material_ui/material_ui.dart';

import '../../features/auth/screens/login_screen.dart';
import '../../features/auth/screens/register_screen.dart';
import '../../features/auth/state/auth_provider.dart';
import '../../features/home/screens/home_screen.dart';
import '../../features/home/screens/role_home_page.dart';

GoRouter createRouter(AuthProvider auth) {
  return GoRouter(
    initialLocation: '/',
    refreshListenable: auth,
    redirect: (context, state) {
      if (auth.isInitializing) {
        return null;
      }

      final location = state.matchedLocation;
      final isAuthRoute = location == '/login' || location == '/register';

      if (!auth.isAuthenticated) {
        return isAuthRoute ? null : '/login';
      }

      if (isAuthRoute || location == '/') {
        return _homeForRole(auth.user?.role);
      }

      return null;
    },
    routes: [
      GoRoute(
        path: '/',
        builder: (context, state) => const SplashPage(),
      ),
      GoRoute(
        path: '/login',
        builder: (context, state) => const LoginScreen(),
      ),
      GoRoute(
        path: '/register',
        builder: (context, state) => const RegisterScreen(),
      ),
      GoRoute(
        path: '/home',
        builder: (context, state) => const HomeScreen(),
      ),
      GoRoute(
        path: '/restaurant',
        builder: (context, state) => const RoleHomePage(role: 'Restaurant'),
      ),
      GoRoute(
        path: '/delivery',
        builder: (context, state) => const RoleHomePage(role: 'DeliveryPerson'),
      ),
    ],
  );
}

String _homeForRole(String? role) {
  switch (role) {
    case 'Restaurant':
      return '/restaurant';
    case 'DeliveryPerson':
      return '/delivery';
    default:
      return '/home';
  }
}

class SplashPage extends StatelessWidget {
  const SplashPage({super.key});

  @override
  Widget build(BuildContext context) {
    return const Scaffold(
      body: Center(child: CircularProgressIndicator()),
    );
  }
}
