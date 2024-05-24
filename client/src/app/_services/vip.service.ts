import { Injectable } from '@angular/core';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { HttpClient } from '@angular/common/http';
import { VisitParams } from '../_models/visitParams';
import { AccountService } from './account.service';
import { map, of, take } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class VipService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map();
  visitParams: VisitParams | undefined;
  user : User | undefined;

  constructor(private http: HttpClient, private accountService: AccountService){
    this.accountService.currentUser$.pipe(take(1)).subscribe({
    next: user => {
      if (user) {
        this.visitParams = new VisitParams();
        this.user = user;
      }
    }
  })
 }

  getVisitParams(){
  return this.visitParams;
  }

  setVisitParams(params: VisitParams){
  this.visitParams = params;
  }

  resetVisitParams(){
  if(this.user){
    this.visitParams = new VisitParams();
    return this.visitParams;
  }
  return;
  }

  addVisit(username: string){
    return this.http.post(this.baseUrl + 'visits/' + username , {});
  }

  getVisits(visitParams: VisitParams) {
    const response = this.memberCache.get(Object.values(visitParams).join('-'))

    if(response) return of(response);

    let params = getPaginationHeaders(visitParams.pageNumber, visitParams.pageSize);

    params = params.append('orderBy', visitParams.orderBy);
    params = params.append('predicate', visitParams.predicate);

    return getPaginatedResult<Member[]>(this.baseUrl + 'visits', params, this.http).pipe(
      map(response => {
        this.memberCache.set(Object.values(visitParams).join('-'), response);
        return response;
      })
    );
  }
}
