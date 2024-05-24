import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input,OnInit, ViewChild } from '@angular/core';
import { TimeagoModule } from 'ngx-timeago';
import { MessageService } from 'src/app/_services/message.service';
import { FormsModule, NgForm } from '@angular/forms';
import { take } from 'rxjs';
import { VipService } from 'src/app/_services/vip.service';
import { AccountService } from 'src/app/_services/account.service';
import { User } from 'src/app/_models/user';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  standalone: true,
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
  imports: [CommonModule, TimeagoModule, FormsModule]
})
export class MemberMessagesComponent implements OnInit{
  @ViewChild('messageForm') messageForm?: NgForm;
  @Input() username?: string;
  messageContent = "";
  loading = false;

  constructor(public messageService: MessageService, private vipService: VipService){  }

  ngOnInit(): void {
    if (!this.username){
      return
    };
      this.vipService.addVisit(this.username);
  }

  sendMessage() {
    if (!this.username) return;
    this.loading = true;
    this.messageService.sendMessage(this.username, this.messageContent).then(() => {
      this.messageForm?.reset();
    }).finally(() => this.loading = false);
  }
}
