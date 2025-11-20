import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  CheckInApiService,
  Team,
  CreateCheckInResponse
} from '../check-in-api.service';
import en from '../../../i18n/en.json';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-check-in',
  templateUrl: './check-in.component.html',
  styleUrls: ['./check-in.component.css']
})
export class CheckInComponent implements OnInit {

  currentStep: number = 1;

  // Strongly-typed form model
  form: CheckInForm = {
    entranceId: '',
    fullName: '',
    email: '',
    companyName: '',
    teamId: null,
    acceptedPolicies: false
  };

  teams: Team[] = [];

  entranceReadonly: boolean = false;
  entranceError: string | null = null;

  submitting: boolean = false;
  submitError: string | null = null;
  checkInResult: CreateCheckInResponse | null = null;

  // expose translations for template use (direct JSON import)
  public t = (en as any).CHECKIN;

  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private api: CheckInApiService
  ) { }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnInit(): void {
    //../checkin?entranceId=ENTRANCE-LOBBY-1
    const qrEntranceId = this.route.snapshot.queryParamMap.get('entranceId');

    if (qrEntranceId) {
      this.form.entranceId = qrEntranceId;
      this.entranceReadonly = true;
      this.onValidateEntrance();
    }
  }

  // STEP 1
  onValidateEntrance(): void {
    this.entranceError = null;

    if (!this.form.entranceId) {
      this.entranceError = this.t.ENTRANCE_REQUIRED;
      return;
    }

    this.api.validateEntrance(this.form.entranceId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: res => {
          if (res.isValid) {
            this.currentStep = 2;
          } else {
            this.entranceError = this.t.ENTRANCE_INVALID;
          }
        },
        error: _ => {
          this.entranceError = this.t.ENTRANCE_VALIDATE_ERROR;
        }
      });
  }

  // STEP 2
  goFromDetailsToTeam(): void {
    if (!this.form.fullName) {
      alert(this.t.FULLNAME_REQUIRED);
      return;
    }

    if (!this.form.email) {
      alert(this.t.EMAIL_REQUIRED);
      return;
    }

    this.currentStep = 3;
    this.loadTeams();
    console.log('Details OK, go to step 3');
  }

  // Takımları yükle
  loadTeams(): void {
    if (this.teams.length > 0) {
      return;
    }

    this.api.getTeams()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: t => {
          this.teams = t;
        },
        error: _ => {
          console.error('Failed to load teams');
        }
      });
  }

  // STEP 3 -> STEP 4
  goFromTeamToSummary(): void {
    if (!this.form.teamId) {
      alert(this.t.TEAM_REQUIRED);
      return;
    }

    if (!this.form.acceptedPolicies) {
      alert(this.t.ACCEPT_POLICIES_REQUIRED);
      return;
    }

    this.currentStep = 4;
    console.log('Team & policies OK, go to step 4');
  }

  // STEP 4: API'ye POST
  submitCheckIn(): void {
    this.submitError = null;
    this.checkInResult = null;
    this.submitting = true;

    const body = {
      entranceId: this.form.entranceId,
      fullName: this.form.fullName,
      email: this.form.email,
      companyName: this.form.companyName,
      teamId: this.form.teamId,
      acceptedPolicies: this.form.acceptedPolicies
    };

    this.api.createCheckIn(body)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: res => {
          this.submitting = false;
          this.checkInResult = res;
          console.log('Check-in created', res);
        },
        error: err => {
          this.submitting = false;
          this.submitError = err?.error?.error || this.t.CREATE_CHECKIN_ERROR;
          console.error('Create check-in failed', err);
        }
      });
  }
}

// Local types
interface CheckInForm {
  entranceId: string;
  fullName: string;
  email: string;
  companyName: string;
  teamId: number | null;
  acceptedPolicies: boolean;
}