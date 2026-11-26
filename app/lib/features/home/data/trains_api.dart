import 'package:dio/dio.dart';

import '../../../core/network/api_client.dart';
import '../models/train.dart';

class TrainsApi {
  TrainsApi({Dio? dio}) : _dio = dio ?? ApiClient.dio;

  final Dio _dio;

  Future<List<Train>> getTrains() async {
    final response = await _dio.get<List<dynamic>>('/trains');
    final data = response.data ?? const <dynamic>[];
    return data
        .map((item) => Train.fromJson(item as Map<String, dynamic>))
        .toList();
  }
}
