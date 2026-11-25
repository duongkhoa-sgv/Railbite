import 'package:flutter_test/flutter_test.dart';
import 'package:railbite_app/main.dart';

void main() {
  testWidgets('Railbite app renders', (tester) async {
    await tester.pumpWidget(const RailbiteApp());
    await tester.pumpAndSettle();
    expect(find.text('Railbite'), findsOneWidget);
  });
}
