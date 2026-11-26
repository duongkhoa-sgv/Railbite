import 'package:dio/dio.dart';

import '../../../core/network/api_client.dart';
import '../models/auth_response.dart';
import '../models/user_profile.dart';

class AuthApi {
  AuthApi({Dio? dio}) : _dio = dio ?? ApiClient.dio;

  final Dio _dio;

  Future<AuthResponse> login({
    required String email,
    required String password,
  }) async {
    final response = await _dio.post<Map<String, dynamic>>(
      '/auth/login',
      data: {'email': email, 'password': password},
    );
    return AuthResponse.fromJson(response.data ?? const <String, dynamic>{});
  }

  Future<AuthResponse> register({
    required String name,
    required String email,
    required String phone,
    required String password,
    String? address,
  }) async {
    final response = await _dio.post<Map<String, dynamic>>(
      '/auth/register',
      data: {
        'name': name,
        'email': email,
        'phone': phone,
        'password': password,
        'address': address,
      },
    );
    return AuthResponse.fromJson(response.data ?? const <String, dynamic>{});
  }

  Future<UserProfile> profile() async {
    final response = await _dio.get<Map<String, dynamic>>('/auth/profile');
    return UserProfile.fromJson(response.data ?? const <String, dynamic>{});
  }

  Future<void> changePassword({
    required String currentPassword,
    required String newPassword,
  }) async {
    await _dio.post<void>(
      '/auth/change-password',
      data: {
        'currentPassword': currentPassword,
        'newPassword': newPassword,
      },
    );
  }
}
