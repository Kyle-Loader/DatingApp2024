import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input,OnInit, ViewChild } from '@angular/core';
import { Message } from 'src/app/_models/message';
import { TimeagoClock, TimeagoModule } from 'ngx-timeago';
import { MessageService } from 'src/app/_services/message.service';
import { FormsModule, NgForm } from '@angular/forms';

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

  constructor(public messageService: MessageService){

  }

  ngOnInit(): void {

  }

  sendMessage() {
    if (!this.username) return;
    this.loading = true;
    this.messageService.sendMessage(this.username, this.messageContent).then(() => {
      this.messageForm?.reset();
    }).finally(() => this.loading = false);
  }
}
