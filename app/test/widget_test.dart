import 'package:flutter_test/flutter_test.dart';
import 'package:railbite_app/main.dart';
import 'package:shared_preferences/shared_preferences.dart';

void main() {
  testWidgets('Railbite app redirects to login when unauthenticated',
      (tester) async {
    SharedPreferences.setMockInitialValues(<String, Object>{});

    await tester.pumpWidget(const RailbiteApp());
    await tester.pump();
    await tester.pump(const Duration(milliseconds: 300));

    expect(find.text('Đăng nhập'), findsWidgets);
  });
}
