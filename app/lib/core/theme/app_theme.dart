import 'package:material_ui/material_ui.dart';

class AppTheme {
  static const Color seed = Color(0xFF1565C0);

  static ThemeData get light => ThemeData(
        useMaterial3: true,
        colorScheme: ColorScheme.fromSeed(seedColor: seed),
        scaffoldBackgroundColor: const Color(0xFFF7F8FA),
        appBarTheme: const AppBarTheme(centerTitle: true),
      );
}
