import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:flutter/foundation.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../core/network/api_client.dart';
import '../data/auth_api.dart';
import '../models/auth_response.dart';
import '../models/user_profile.dart';

class AuthProvider extends ChangeNotifier {
  AuthProvider({AuthApi? api}) : _api = api ?? AuthApi();

  static const _tokenKey = 'auth_token';
  static const _userKey = 'auth_user';

  final AuthApi _api;

  String? _token;
  UserProfile? _user;
  bool _initializing = true;
  bool _busy = false;
  String? _error;

  String? get token => _token;
  UserProfile? get user => _user;
  bool get isAuthenticated => _token != null && _user != null;
  bool get isInitializing => _initializing;
  bool get isBusy => _busy;
  String? get error => _error;

  Future<void> bootstrap() async {
    final prefs = await SharedPreferences.getInstance();
    _token = prefs.getString(_tokenKey);

    final cachedUser = prefs.getString(_userKey);
    if (cachedUser != null) {
      try {
        _user = UserProfile.fromJson(
          jsonDecode(cachedUser) as Map<String, dynamic>,
        );
      } catch (_) {
        _user = null;
      }
    }

    if (_token != null) {
      ApiClient.setToken(_token!);
      try {
        _user = await _api.profile();
        await _persistUser(_user!);
      } on DioException catch (error) {
        if (error.response?.statusCode == 401) {
          await _clearSession();
        }
      }
    }

    _initializing = false;
    notifyListeners();
  }

  Future<bool> login(String email, String password) async {
    _setBusy(true);
    try {
      final response = await _api.login(
        email: email.trim(),
        password: password,
      );
      await _apply(response);
      return true;
    } catch (error) {
      _error = _message(error);
      return false;
    } finally {
      _setBusy(false);
    }
  }

  Future<bool> register({
    required String name,
    required String email,
    required String phone,
    required String password,
    String? address,
  }) async {
    _setBusy(true);
    try {
      final response = await _api.register(
        name: name.trim(),
        email: email.trim(),
        phone: phone.trim(),
        password: password,
        address: (address == null || address.trim().isEmpty) ? null : address.trim(),
      );
      await _apply(response);
      return true;
    } catch (error) {
      _error = _message(error);
      return false;
    } finally {
      _setBusy(false);
    }
  }

  Future<void> logout() async {
    await _clearSession();
    notifyListeners();
  }

  void clearError() {
    _error = null;
    notifyListeners();
  }

  Future<void> _apply(AuthResponse response) async {
    _token = response.token;
    _user = response.user;
    _error = null;
    ApiClient.setToken(response.token);

    final prefs = await SharedPreferences.getInstance();
    await prefs.setString(_tokenKey, response.token);
    await _persistUser(response.user, prefs: prefs);
    notifyListeners();
  }

  Future<void> _persistUser(
    UserProfile user, {
    SharedPreferences? prefs,
  }) async {
    final store = prefs ?? await SharedPreferences.getInstance();
    await store.setString(_userKey, jsonEncode(user.toJson()));
  }

  Future<void> _clearSession() async {
    _token = null;
    _user = null;
    _error = null;
    ApiClient.clearToken();

    final prefs = await SharedPreferences.getInstance();
    await prefs.remove(_tokenKey);
    await prefs.remove(_userKey);
  }

  void _setBusy(bool value) {
    _busy = value;
    if (value) {
      _error = null;
    }
    notifyListeners();
  }

  String _message(Object error) {
    if (error is DioException) {
      final data = error.response?.data;
      if (data is Map<String, dynamic>) {
        final detail = data['detail'];
        if (detail is String && detail.isNotEmpty) {
          return detail;
        }
        final title = data['title'];
        if (title is String && title.isNotEmpty) {
          return title;
        }
      }

      if (error.type == DioExceptionType.connectionError ||
          error.type == DioExceptionType.connectionTimeout) {
        return 'Không kết nối được server. Kiểm tra API đang chạy.';
      }

      return error.message ?? 'Đã có lỗi xảy ra.';
    }

    return error.toString();
  }
}
