import { Component } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { VisitParams } from 'src/app/_models/visitParams';
import { VipService } from 'src/app/_services/vip.service';

@Component({
  selector: 'app-visits',
  templateUrl: './visits.component.html',
  styleUrls: ['./visits.component.css']
})
export class VisitsComponent {
  members: Member[] | undefined;
  visitParams: VisitParams | undefined;
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination | undefined;

  constructor(private vipService: VipService) {
    this.visitParams = this.vipService.getVisitParams();
  }

  ngOnInit(){
    this.loadVisits()
  }

  loadVisits() {
    if(this.visitParams){
      this.vipService.getVisits(this.visitParams).subscribe({
        next: response => {
          if (response.result && response.pagination) {
            this.members = response.result;
            this.pagination = response.pagination;
          }
        }
      })
    }
  }

  resetFilters(){
    this.visitParams = this.vipService.resetVisitParams();
    this.loadVisits();
  }

  pageChanged(event: any) {
    if (this.visitParams && this.visitParams?.pageNumber !== event.page){
      this.visitParams.pageNumber = event.page;
      this.vipService.setVisitParams(this.visitParams);
      this.loadVisits();
    }
  }
}
