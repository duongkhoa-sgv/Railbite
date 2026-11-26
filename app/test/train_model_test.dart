import 'package:flutter_test/flutter_test.dart';
import 'package:railbite_app/features/home/models/train.dart';

void main() {
  test('Train.fromJson parses nested stops', () {
    final train = Train.fromJson(<String, dynamic>{
      'id': 't1',
      'trainNumber': '12627',
      'trainName': 'Karnataka Express',
      'stops': <dynamic>[
        <String, dynamic>{
          'id': 's1',
          'stationName': 'KSR Bengaluru',
          'stopOrder': 1,
          'arrivalTime': '06:00:00',
          'departureTime': '06:10:00',
          'isDeliveryAvailable': true,
        },
      ],
    });

    expect(train.trainNumber, '12627');
    expect(train.stops, hasLength(1));
    expect(train.stops.first.stationName, 'KSR Bengaluru');
    expect(train.stops.first.timeLabel, '06:00');
    expect(train.stops.first.isDeliveryAvailable, isTrue);
  });

  test('TrainStop handles missing times', () {
    final stop = TrainStop.fromJson(<String, dynamic>{
      'id': 's2',
      'stationName': 'Mysuru',
      'stopOrder': 2,
      'isDeliveryAvailable': false,
    });

    expect(stop.timeLabel, '--:--');
    expect(stop.isDeliveryAvailable, isFalse);
  });
}
