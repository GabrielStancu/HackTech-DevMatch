import { Component } from '@angular/core';
import { MatchingService } from 'src/app/services/matching.service';
import { UploadService } from 'src/app/services/upload.service';

@Component({
  selector: 'app-find-job',
  templateUrl: './find-job.component.html',
  styleUrls: ['./find-job.component.css']
})
export class FindJobComponent {
  // Existing properties and methods
  matchedJobs: any[] = [];
  displayedColumns: string[] = ['job_name', 'score', 'reasoning'];
  loadingJobs: boolean = false;
  errorLoadingJobs: string = '';
  currentCvId: string = '';

  // Properties for the FAQ Chat
  isChatVisible: boolean = false;
  userQuestion: string = '';
  chatMessages: { sender: string, message: string }[] = [];
  hideChatTimeout: any;

  constructor(private matchingService: MatchingService, private uploadService: UploadService) { }

  // Methods to control chat visibility
  showChat(): void {
    clearTimeout(this.hideChatTimeout);
    this.isChatVisible = true;
  }

  scheduleHideChat(): void {
    this.hideChatTimeout = setTimeout(() => {
      this.isChatVisible = false;
    }, 300); // Delay to allow moving cursor to chat window
  }

  // Method to send the user's question
  sendQuestion(): void {
    if (this.userQuestion.trim()) {
      // Add the user's question to the chat
      this.chatMessages.push({ sender: 'user', message: this.userQuestion });

      // Send the question to the backend
      this.matchingService.sendFaqQuestion(this.userQuestion).subscribe(
        (response) => {
          // Add the bot's response to the chat
          this.chatMessages.push({ sender: 'bot', message: response.answer });
        },
        (error) => {
          console.error('Error sending FAQ question:', error);
          // Handle error, possibly add an error message to the chat
          this.chatMessages.push({ sender: 'bot', message: 'Sorry, an error occurred.' });
        }
      );

      // Clear the input field after sending
      this.userQuestion = '';
    }
  }

  onGetMatchedJobs(): void {
    this.loadingJobs = true;
    this.errorLoadingJobs = '';
    this.matchedJobs = [];

    const requestData = {
      CvId: this.currentCvId,
    };

    this.matchingService.getMatchedJobs(requestData).subscribe(
      (jobs) => {
        this.matchedJobs = jobs;
        this.loadingJobs = false;
        this.fetchReasoningForCandidates(jobs);
      },
      (error) => {
        console.error('Error fetching matching jobs:', error);
        this.errorLoadingJobs = 'An error occurred while fetching matching jobs.';
        this.loadingJobs = false;
      }
    );
  }

  private fetchReasoningForCandidates(jobs: any[]): void {
    // Map each candidate to a reasoning request
    const reasoningRequests = jobs.map(candidate => {
        // Create the requestData object for each candidate
        const requestData = {
            reasoning_1: candidate.general_reasoning,
            reasoning_2: candidate.tehnical_reasoning,
            reasoning_3: candidate.domain_reasoning,
            direction: "job2cvs"
        };

        // Return the observable from getFinalReasoning for this candidate
        return this.matchingService.getFinalReasoning(requestData).toPromise();
    });

    // Wait for all requests to complete
    Promise.all(reasoningRequests).then((responses) => {
        this.matchedJobs = jobs.map((job, index) => ({
            ...job,
            reasoning: responses[index].reasoning
        }));
    }).catch(error => {
        console.error('Error fetching reasoning:', error);
        this.errorLoadingJobs = 'An error occurred while fetching candidate reasoning.';
    });
  }

  public receiveFile(file: File) {
    this.uploadService.uploadCv(file).subscribe(result => {
        this.currentCvId = result.id;
      }
    );
  }
}
