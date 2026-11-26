import 'user_profile.dart';

class AuthResponse {
  const AuthResponse({
    required this.token,
    required this.expiresAtUtc,
    required this.user,
  });

  final String token;
  final DateTime expiresAtUtc;
  final UserProfile user;

  factory AuthResponse.fromJson(Map<String, dynamic> json) {
    return AuthResponse(
      token: json['token'] as String? ?? '',
      expiresAtUtc: DateTime.tryParse(json['expiresAtUtc'] as String? ?? '') ??
          DateTime.now().toUtc(),
      user: UserProfile.fromJson(
        (json['user'] as Map<String, dynamic>?) ?? const <String, dynamic>{},
      ),
    );
  }
}
