import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

export interface Team {
  id: number;
  name: string;
}

export interface ValidateEntranceResponse {
  entranceId: string;
  isValid: boolean;
}

export interface CreateCheckInResponse {
  id: string;
  fullName: string;
  email: string;
  companyName: string;
  teamId: number;
  teamName: string;
  entranceId: string;
  checkInDate: string;
}

@Injectable({
  providedIn: 'root'
})
export class CheckInApiService {

  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) { }

  validateEntrance(entranceId: string): Observable<ValidateEntranceResponse> {
    return this.http.post<ValidateEntranceResponse>(
      this.baseUrl + '/entrances/validate',
      { entranceId: entranceId }
    );
  }

  getTeams(): Observable<Team[]> {
    return this.http.get<Team[]>(this.baseUrl + '/teams');
  }

  createCheckIn(body: any): Observable<CreateCheckInResponse> {
    return this.http.post<CreateCheckInResponse>(
      this.baseUrl + '/checkins',
      body
    );
  }
}