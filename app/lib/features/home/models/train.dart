class Train {
  const Train({
    required this.id,
    required this.trainNumber,
    required this.trainName,
    required this.stops,
  });

  final String id;
  final String trainNumber;
  final String trainName;
  final List<TrainStop> stops;

  factory Train.fromJson(Map<String, dynamic> json) {
    final rawStops = json['stops'] as List<dynamic>? ?? const <dynamic>[];
    return Train(
      id: json['id'] as String? ?? '',
      trainNumber: json['trainNumber'] as String? ?? '',
      trainName: json['trainName'] as String? ?? '',
      stops: rawStops
          .map((item) => TrainStop.fromJson(item as Map<String, dynamic>))
          .toList(),
    );
  }
}

class TrainStop {
  const TrainStop({
    required this.id,
    required this.stationName,
    required this.stopOrder,
    required this.isDeliveryAvailable,
    this.arrivalTime,
    this.departureTime,
  });

  final String id;
  final String stationName;
  final int stopOrder;
  final bool isDeliveryAvailable;
  final String? arrivalTime;
  final String? departureTime;

  factory TrainStop.fromJson(Map<String, dynamic> json) {
    return TrainStop(
      id: json['id'] as String? ?? '',
      stationName: json['stationName'] as String? ?? '',
      stopOrder: (json['stopOrder'] as num?)?.toInt() ?? 0,
      isDeliveryAvailable: json['isDeliveryAvailable'] as bool? ?? false,
      arrivalTime: json['arrivalTime'] as String?,
      departureTime: json['departureTime'] as String?,
    );
  }

  String get timeLabel {
    final value = arrivalTime ?? departureTime;
    if (value == null || value.length < 5) {
      return '--:--';
    }
    return value.substring(0, 5);
  }
}
