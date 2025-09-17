# Hopewell Clinic API - Frontend Integration Guide

## 🚀 **Base URL**
```
http://localhost:5002
```

## 📋 **Complete API Endpoints Reference**

### **1. Appointments Management**

#### Get All Appointments
```http
GET /api/Appointments
```

#### Get Appointment by ID
```http
GET /api/Appointments/{id}
```

#### Get Patient's Appointments
```http
GET /api/Appointments/patient/{patientId}
```

#### Create New Appointment
```http
POST /api/Appointments
Content-Type: application/json

{
  "patientId": "guid",
  "serviceId": "guid",
  "appointmentDate": "2024-01-15",
  "startTime": "10:00",
  "staffId": "guid", // optional
  "notes": "string" // optional
}
```

#### Update Appointment
```http
PUT /api/Appointments/{id}
Content-Type: application/json

{
  "appointmentDate": "2024-01-15",
  "startTime": "10:00",
  "endTime": "10:30",
  "notes": "Updated notes",
  "status": "confirmed"
}
```

#### Update Appointment Status
```http
PUT /api/Appointments/{id}/status
Content-Type: application/json

{
  "status": "completed" // or "cancelled", "in-progress", etc.
}
```

#### Get Today's Appointments
```http
GET /api/Appointments/today
```

#### Get Available Time Slots
```http
GET /api/Appointments/available-slots?date=2024-01-15&serviceId=guid
```

#### Assign Staff to Appointment
```http
POST /api/Appointments/{id}/assign-staff
Content-Type: application/json

{
  "staffId": "guid"
}
```

#### Cancel Appointment
```http
DELETE /api/Appointments/{id}
```

### **2. Patient Management**

#### Get All Patients
```http
GET /api/Patients
```

#### Get Patient by ID
```http
GET /api/Patients/{id}
```

#### Update Patient Information
```http
PUT /api/Patients/{id}
Content-Type: application/json

{
  "address": "New Address",
  "phoneNumber": "+1234567890"
}
```

#### Search Patients
```http
GET /api/Patients/search?query=John
```

### **3. Services Management**

#### Get All Services
```http
GET /api/Services
```

#### Create New Service (Admin Only)
```http
POST /api/Services
Content-Type: application/json

{
  "name": "Service Name",
  "description": "Service Description",
  "durationMinutes": 30
}
```

#### Update Service (Admin Only)
```http
PUT /api/Services/{id}
Content-Type: application/json

{
  "name": "Updated Name",
  "description": "Updated Description",
  "durationMinutes": 45,
  "isActive": true
}
```

#### Deactivate Service (Admin Only)
```http
DELETE /api/Services/{id}
```

### **4. Staff Management**

#### Get All Staff
```http
GET /api/Staff
```

#### Get Staff by ID
```http
GET /api/Staff/{id}
```

#### Get Staff by Role
```http
GET /api/Staff/by-role/{role}
```

#### Get Staff Schedule
```http
GET /api/Staff/{id}/schedule?startDate=2024-01-01&endDate=2024-01-31
```

#### Get Staff Availability
```http
GET /api/Staff/{id}/availability?date=2024-01-15
```

#### Update Staff Information
```http
PUT /api/Staff/{id}
Content-Type: application/json

{
  "phoneNumber": "+1234567890"
}
```

#### Update Staff Availability
```http
POST /api/Staff/{id}/availability
Content-Type: application/json

{
  "dayOfWeek": 1, // Monday = 1, Sunday = 7
  "startTime": "09:00",
  "endTime": "17:00"
}
```

### **5. Doctor Dashboard**

#### Get Doctor's Patients
```http
GET /api/Doctor/{doctorId}/patients
```

#### Get Doctor's Upcoming Appointments
```http
GET /api/Doctor/{doctorId}/appointments/upcoming
```

#### Create Walk-in Appointment
```http
POST /api/Doctor/appointments/walkin
Content-Type: application/json

{
  "patientFirstName": "John",
  "patientLastName": "Doe",
  "patientPhone": "+1234567890",
  "doctorId": "guid",
  "serviceId": "guid",
  "appointmentDate": "2024-01-15",
  "startTime": "10:00"
}
```

### **6. Nurse Dashboard**

#### Get Today's Appointments
```http
GET /api/Nurse/appointments/today
```

#### Search Patients
```http
GET /api/Nurse/patients/search?query=John
```

#### Book Appointment for Patient
```http
POST /api/Nurse/appointments/book-for-patient
Content-Type: application/json

{
  "patientId": "guid",
  "staffId": "guid", // optional
  "serviceId": "guid",
  "appointmentDate": "2024-01-15",
  "startTime": "10:00"
}
```

### **7. Admin Dashboard**

#### Create Staff User
```http
POST /api/Admin/create-staff
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@clinic.com",
  "password": "SecurePassword123",
  "role": "doctor" // or "nurse", "admin"
}
```

#### Update User Status
```http
PUT /api/Admin/users/{userId}
Content-Type: application/json

{
  "isActive": true
}
```

#### Get All Users
```http
GET /api/Admin/users
```

#### Get All Roles
```http
GET /api/Admin/roles
```

#### Update User Role
```http
PUT /api/Admin/users/{userId}/role
Content-Type: application/json

{
  "newRole": "nurse"
}
```

#### Get Appointment Statistics
```http
GET /api/Admin/reports/appointment-stats?startDate=2024-01-01&endDate=2024-01-31
```

#### Get Revenue Report
```http
GET /api/Admin/reports/revenue?startDate=2024-01-01&endDate=2024-01-31
```

## 🔐 **Authentication Integration**

### JWT Token Usage
All endpoints that require authentication should include the JWT token in the Authorization header:

```http
Authorization: Bearer {your-jwt-token}
```

### Login Endpoint
```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password"
}
```

### Register Endpoint
```http
POST /api/Auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890",
  "userType": "patient"
}
```

## 📱 **Frontend Implementation Examples**

### React/JavaScript Example
```javascript
// API Service Class
class HopewellClinicAPI {
  constructor(baseURL = 'http://localhost:5002') {
    this.baseURL = baseURL;
    this.token = localStorage.getItem('authToken');
  }

  async request(endpoint, options = {}) {
    const url = `${this.baseURL}${endpoint}`;
    const config = {
      headers: {
        'Content-Type': 'application/json',
        ...(this.token && { Authorization: `Bearer ${this.token}` }),
        ...options.headers,
      },
      ...options,
    };

    const response = await fetch(url, config);
    
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    
    return response.json();
  }

  // Appointments
  async getAppointments() {
    return this.request('/api/Appointments');
  }

  async createAppointment(appointmentData) {
    return this.request('/api/Appointments', {
      method: 'POST',
      body: JSON.stringify(appointmentData),
    });
  }

  async getTodaysAppointments() {
    return this.request('/api/Appointments/today');
  }

  // Patients
  async getPatients() {
    return this.request('/api/Patients');
  }

  async searchPatients(query) {
    return this.request(`/api/Patients/search?query=${encodeURIComponent(query)}`);
  }

  // Services
  async getServices() {
    return this.request('/api/Services');
  }

  // Admin
  async getAppointmentStats(startDate, endDate) {
    return this.request(`/api/Admin/reports/appointment-stats?startDate=${startDate}&endDate=${endDate}`);
  }
}

// Usage Example
const api = new HopewellClinicAPI();

// Get all services
api.getServices().then(services => {
  console.log('Available services:', services);
});

// Create an appointment
api.createAppointment({
  patientId: 'patient-guid',
  serviceId: 'service-guid',
  appointmentDate: '2024-01-15',
  startTime: '10:00'
}).then(response => {
  console.log('Appointment created:', response);
});
```

### Vue.js Example
```javascript
// Vue.js Service
export const hopewellAPI = {
  baseURL: 'http://localhost:5002',
  
  async request(endpoint, options = {}) {
    const token = localStorage.getItem('authToken');
    const response = await fetch(`${this.baseURL}${endpoint}`, {
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` }),
        ...options.headers,
      },
      ...options,
    });
    
    return response.json();
  },

  // Appointments
  getAppointments() {
    return this.request('/api/Appointments');
  },

  createAppointment(data) {
    return this.request('/api/Appointments', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },

  // Patients
  getPatients() {
    return this.request('/api/Patients');
  },

  searchPatients(query) {
    return this.request(`/api/Patients/search?query=${query}`);
  }
};
```

## 🎯 **Dashboard-Specific Endpoints**

### Patient Dashboard
- `GET /api/Appointments/patient/{patientId}` - View own appointments
- `POST /api/Appointments` - Book new appointment

### Doctor Dashboard
- `GET /api/Doctor/{doctorId}/patients` - View assigned patients
- `GET /api/Doctor/{doctorId}/appointments/upcoming` - View upcoming appointments
- `POST /api/Doctor/appointments/walkin` - Create walk-in appointments
- `PUT /api/Appointments/{id}/status` - Update appointment status

### Nurse Dashboard
- `GET /api/Appointments/today` - View today's schedule
- `GET /api/Patients/search?query=name` - Search patients
- `POST /api/Nurse/appointments/book-for-patient` - Book appointments for patients

### Admin Dashboard
- `GET /api/Admin/users` - Manage users
- `POST /api/Admin/create-staff` - Create staff accounts
- `GET /api/Admin/reports/appointment-stats` - View statistics
- `GET /api/Admin/reports/revenue` - View revenue reports
- `POST /api/Services` - Manage services

## 🚨 **Error Handling**

All endpoints return consistent error responses:

```json
{
  "error": "Error message",
  "details": "Additional error details (optional)"
}
```

Common HTTP status codes:
- `200` - Success
- `201` - Created
- `400` - Bad Request
- `401` - Unauthorized
- `404` - Not Found
- `500` - Internal Server Error

## 📊 **Response Formats**

### Appointment Response
```json
{
  "id": "guid",
  "appointmentDate": "2024-01-15",
  "startTime": "10:00",
  "endTime": "10:30",
  "status": "confirmed",
  "notes": "string",
  "service": {
    "id": "guid",
    "name": "General Consultation",
    "description": "string",
    "durationMinutes": 30
  },
  "patient": {
    "id": "guid",
    "firstName": "John",
    "lastName": "Doe",
    "phone": "+1234567890"
  },
  "staff": {
    "id": "guid",
    "firstName": "Dr. Jane",
    "lastName": "Smith",
    "role": "doctor"
  }
}
```

### Patient Response
```json
{
  "id": "guid",
  "userId": "guid",
  "patientNumber": "PAT001",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890",
  "email": "john@example.com",
  "dateOfBirth": "1990-01-01",
  "address": "123 Main St",
  "emergencyContactName": "Jane Doe",
  "emergencyContactPhone": "+1234567891",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

## 🔧 **Testing**

Use Swagger UI for interactive testing:
```
http://localhost:5002/swagger
```

Or use the provided PowerShell test script:
```powershell
.\test_data_creation.ps1
```

## 📝 **Notes**

1. All dates should be in ISO 8601 format (YYYY-MM-DD)
2. All times should be in 24-hour format (HH:mm)
3. GUIDs are used for all entity IDs
4. All endpoints support CORS for frontend integration
5. Authentication is handled via JWT tokens
6. Role-based access control is implemented for sensitive endpoints

