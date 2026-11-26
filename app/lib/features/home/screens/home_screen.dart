import 'package:material_ui/material_ui.dart';
import 'package:provider/provider.dart';

import '../../auth/state/auth_provider.dart';
import '../data/trains_api.dart';
import '../models/train.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  final TrainsApi _api = TrainsApi();
  late Future<List<Train>> _trainsFuture;
  TrainStop? _selectedStop;

  @override
  void initState() {
    super.initState();
    _trainsFuture = _api.getTrains();
  }

  void _reload() {
    setState(() {
      _trainsFuture = _api.getTrains();
      _selectedStop = null;
    });
  }

  void _selectStop(TrainStop stop) {
    setState(() => _selectedStop = stop);
  }

  @override
  Widget build(BuildContext context) {
    final auth = context.watch<AuthProvider>();
    final user = auth.user;
    final balance = user?.walletBalance;

    return Scaffold(
      appBar: AppBar(
        title: const Text('Railbite'),
        actions: [
          IconButton(
            tooltip: 'Đăng xuất',
            onPressed: () async {
              await auth.logout();
            },
            icon: const Icon(Icons.logout),
          ),
        ],
      ),
      body: FutureBuilder<List<Train>>(
        future: _trainsFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          }

          if (snapshot.hasError) {
            return _ErrorView(onRetry: _reload);
          }

          final trains = snapshot.data ?? const <Train>[];
          if (trains.isEmpty) {
            return const Center(child: Text('Chưa có chuyến tàu nào.'));
          }

          return ListView(
            padding: const EdgeInsets.all(16),
            children: [
              Card(
                child: ListTile(
                  leading: const CircleAvatar(child: Icon(Icons.person)),
                  title: Text(user?.name ?? 'Hành khách'),
                  subtitle: Text(user?.email ?? ''),
                  trailing: balance == null
                      ? null
                      : Text('${balance.toStringAsFixed(0)} đ'),
                ),
              ),
              const SizedBox(height: 16),
              const Text(
                'Chọn ga nhận món',
                style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
              ),
              const SizedBox(height: 8),
              ...trains.map(
                (train) => _TrainCard(
                  train: train,
                  selectedStop: _selectedStop,
                  onSelect: _selectStop,
                ),
              ),
            ],
          );
        },
      ),
      bottomNavigationBar: _selectedStop == null
          ? null
          : SafeArea(
              child: Padding(
                padding: const EdgeInsets.all(16),
                child: FilledButton.icon(
                  onPressed: _selectedStop!.isDeliveryAvailable
                      ? () {
                          ScaffoldMessenger.of(context).showSnackBar(
                            SnackBar(
                              content: Text(
                                'Đặt món tại ${_selectedStop!.stationName} '
                                '(mở ở P2)',
                              ),
                            ),
                          );
                        }
                      : null,
                  icon: const Icon(Icons.restaurant),
                  label: Text(
                    _selectedStop!.isDeliveryAvailable
                        ? 'Đặt món tại ${_selectedStop!.stationName}'
                        : 'Ga này không hỗ trợ giao món',
                  ),
                ),
              ),
            ),
    );
  }
}

class _TrainCard extends StatelessWidget {
  const _TrainCard({
    required this.train,
    required this.selectedStop,
    required this.onSelect,
  });

  final Train train;
  final TrainStop? selectedStop;
  final ValueChanged<TrainStop> onSelect;

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.only(bottom: 12),
      child: Padding(
        padding: const EdgeInsets.all(12),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              '${train.trainNumber} · ${train.trainName}',
              style: const TextStyle(fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 8),
            Wrap(
              spacing: 8,
              runSpacing: 8,
              children: train.stops.map((stop) {
                return ChoiceChip(
                  label: Text('${stop.stationName} · ${stop.timeLabel}'),
                  selected: selectedStop?.id == stop.id,
                  onSelected: (_) => onSelect(stop),
                );
              }).toList(),
            ),
          ],
        ),
      ),
    );
  }
}

class _ErrorView extends StatelessWidget {
  const _ErrorView({required this.onRetry});

  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          const Icon(Icons.cloud_off, size: 56),
          const SizedBox(height: 12),
          const Text('Không tải được danh sách tàu.'),
          const SizedBox(height: 12),
          OutlinedButton(
            onPressed: onRetry,
            child: const Text('Thử lại'),
          ),
        ],
      ),
    );
  }
}
