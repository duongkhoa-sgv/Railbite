class UserProfile {
  const UserProfile({
    required this.id,
    required this.name,
    required this.email,
    required this.phone,
    required this.role,
    required this.isActive,
    this.address,
    this.restaurantName,
    this.vehicleNumber,
    this.walletBalance,
  });

  final String id;
  final String name;
  final String email;
  final String phone;
  final String role;
  final bool isActive;
  final String? address;
  final String? restaurantName;
  final String? vehicleNumber;
  final double? walletBalance;

  factory UserProfile.fromJson(Map<String, dynamic> json) {
    return UserProfile(
      id: json['id'] as String? ?? '',
      name: json['name'] as String? ?? '',
      email: json['email'] as String? ?? '',
      phone: json['phone'] as String? ?? '',
      role: json['role'] as String? ?? '',
      isActive: json['isActive'] as bool? ?? true,
      address: json['address'] as String?,
      restaurantName: json['restaurantName'] as String?,
      vehicleNumber: json['vehicleNumber'] as String?,
      walletBalance: (json['walletBalance'] as num?)?.toDouble(),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'name': name,
      'email': email,
      'phone': phone,
      'role': role,
      'isActive': isActive,
      'address': address,
      'restaurantName': restaurantName,
      'vehicleNumber': vehicleNumber,
      'walletBalance': walletBalance,
    };
  }
}
