import { TestBed } from '@angular/core/testing';

import { CheckInApiService } from './check-in-api.service';

describe('CheckInApiService', () => {
  let service: CheckInApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CheckInApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
