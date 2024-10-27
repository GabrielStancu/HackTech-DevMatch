import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatchingService } from 'src/app/services/matching.service';
import { AppAcceptRejectDialogComponent } from '../app-accept-reject-dialog/app-accept-reject-dialog.component';
import { UploadService } from 'src/app/services/upload.service';

interface FilterWeightPair {
  filter: string;
  weight: number | null;
}

@Component({
  selector: 'app-find-devs',
  templateUrl: './find-devs.component.html',
  styleUrls: ['./find-devs.component.css']
})
export class FindDevsComponent {
// Existing properties and methods
matchedDevs: any[] = [];
availableJobs: { id: string, title: string }[] = [];
selectedJob: string | null = null;
displayedColumns: string[] = ['candidate_name', 'score', 'reasoning', 'actions'];
loadingJobs: boolean = false;
loadingDevs: boolean = false;
errorLoadingJobs: string = '';
filterCriteria: string = '';
currentJobId: string = '';

// Properties for the FAQ Chat
isChatVisible: boolean = false;
userQuestion: string = '';
chatMessages: { sender: string, message: string }[] = [];
hideChatTimeout: any;
selectedJobTitle: string = 'Select Job';

newFilter: string = '';
newWeight: number | null = null;
filterWeights: FilterWeightPair[] = [];
weightError: string | null = null;

constructor(private dialog: MatDialog, private matchingService: MatchingService, private uploadService : UploadService) { }

ngOnInit(): void {
  this.fetchAvailableJobs();
}

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

  onGetMatchedDevs(): void {
    var requestData = {}
    this.loadingDevs = true;
    if (this.currentJobId) {
      requestData = {
          ProjectId: this.currentJobId,
          Keywords: this.constructKeywordsObject()
      };
    } else if (this.selectedJob) {
      requestData = {
        ProjectId: this.selectedJob,
        Keywords: this.constructKeywordsObject()
      };
    } else{
      alert('Please upload or select a job to find matching candidates.');
    }

    // Make a request using MatchingService with jobId and filterCriteria
    this.matchingService.getMatchedCandidates(requestData).subscribe(
      (candidates) => {
        this.matchedDevs = candidates;
        this.fetchReasoningForCandidates();
        this.loadingDevs = false;
      },
      (error) => {
        this.errorLoadingJobs = 'An error occurred while fetching matching candidates.';
        console.error(error);
        this.loadingDevs = false;
      }
    );
  }

  private fetchReasoningForCandidates(): void {  
    try {
      for (const candidate of this.matchedDevs) {
        // Create the requestData object for each candidate
        const requestData = {
          reasoning_1: candidate.general_reasoning,
          reasoning_2: candidate.tehnical_reasoning,
          reasoning_3: candidate.domain_reasoning,
          direction: "cv2jobs"
        };
  
        // Await the response from getFinalReasoning
        this.matchingService.getFinalReasoning(requestData).subscribe(result =>{
          candidate.reasoning = result.reasoning;
        });
      }
  
      // Log the reasoning for each candidate after all requests are complete
      console.log('Reasoning for candidates:', this.matchedDevs.map(dev => dev.reasoning));
    } catch (error) {
      console.error('Error fetching reasoning:', error);
      this.errorLoadingJobs = 'An error occurred while fetching candidate reasoning.';
    }
  }
  

  private constructKeywordsObject(): { [key: string]: number } {
    const keywords: { [key: string]: number } = {};

    this.filterWeights.forEach(pair => {
      keywords[pair.filter] = pair.weight || 0; // Assign weight or default to 0
    });

    return keywords;
  }

  fetchAvailableJobs(): void {
    this.matchingService.getAllJobs().subscribe(
      (jobs) => {
        this.availableJobs = jobs;
      },
      (error) => {
        console.error('Error fetching jobs:', error);
      }
    );
  }

  onJobSelectChange(jobId: string): void {
    // Find the selected job title based on the selected job ID
    const selectedJob = this.availableJobs.find(job => job.id === jobId);
    this.selectedJobTitle = selectedJob ? selectedJob.title : 'Select Job';
  }

  sendAcceptEmail(dev: any): void {
    const requestData = {
      reasoning_1: dev.general_reasoning,
      reasoning_2: dev.tehnical_reasoning,
      reasoning_3: dev.domain_reasoning,
      accepted: 'true'
    };
    // Fetch initial response text for "Accept" email from backend
    this.matchingService.getEmailTemplate(requestData).subscribe( response => {
      const dialogRef = this.dialog.open(AppAcceptRejectDialogComponent, {
        width: '400px',
        data: response.email_response
      });

      dialogRef.afterClosed().subscribe((editedMessage: string) => {
        if (editedMessage) {
          const mailRequestData = {
            subject: "Welcome to Our Team",
            content: editedMessage,
          };
          this.matchingService.sendEmail(mailRequestData).subscribe(() => {
            console.log('Accept email sent successfully');
          });
        }
      });
    });
  }

  sendRejectEmail(dev: any): void {
    const requestData = {
      reasoning_1: dev.general_reasoning,
      reasoning_2: dev.tehnical_reasoning,
      reasoning_3: dev.domain_reasoning,
      accepted: 'false'
    };

    // Fetch initial response text for "Reject" email from backend
    this.matchingService.getEmailTemplate(requestData).subscribe(response => {
      const dialogRef = this.dialog.open(AppAcceptRejectDialogComponent, {
        width: '400px',
        data: response.email_response
      });
      dialogRef.afterClosed().subscribe((editedMessage: string) => {
        if (editedMessage) {
          const mailRequestData = {
            subject: "Thank You for Your Application",
            content: editedMessage,
          };
          this.matchingService.sendEmail(mailRequestData).subscribe(() => {
            console.log('Reject email sent successfully');
          });
        }
      });
    });
  }

  public receiveFile(file: File) {
    this.uploadService.uploadJob(file).subscribe(result =>
      this.currentJobId = result.id
    );
  }

  // Add a new filter-weight pair
  addFilterWeightPair(): void {
    // Calculate the total weight of specified filters
    const specifiedWeightTotal = this.filterWeights.reduce((acc, pair) => acc + (pair.weight || 0), 0);
    const specifiedWeight = this.newWeight || 0;

    // Check if adding this weight would exceed 100%
    if (specifiedWeightTotal + specifiedWeight > 100) {
        this.weightError = 'Total weight cannot exceed 100%';
        return;
    }

    // Add the new filter-weight pair to the list
    this.filterWeights.push({ filter: this.newFilter, weight: this.newWeight });
    this.newFilter = '';
    this.newWeight = null;
    this.weightError = null; // Clear any previous error

    // Redistribute the remaining weight among unweighted filters
    this.redistributeRemainingWeight();
  }

  // Remove a filter-weight pair and revalidate weights
  removeFilterWeightPair(index: number): void {
    this.filterWeights.splice(index, 1);
    this.redistributeRemainingWeight(); // Adjust weights after removing
  }

  // Check if weights sum to 100 (used to enable/disable the submit button)
  isWeightValid(): boolean {
    const totalWeight = this.filterWeights.reduce((acc, pair) => acc + (pair.weight || 0), 0);
    return totalWeight === 100;
  }

  // Redistribute remaining weight among unweighted filters dynamically
  private redistributeRemainingWeight(): void {
    // Calculate total specified weight
    const specifiedWeightTotal = this.filterWeights.reduce((acc, pair) => acc + (pair.weight || 0), 0);

    // Identify unweighted filters
    const unweightedFilters = this.filterWeights.filter(pair => pair.weight === null || pair.weight === undefined);

    // Distribute remaining weight equally among unweighted filters if any exist
    if (unweightedFilters.length > 0) {
        const remainingWeight = 100 - specifiedWeightTotal;
        const equalWeight = remainingWeight / unweightedFilters.length;

        // Update weight for each unweighted filter
        unweightedFilters.forEach(pair => pair.weight = equalWeight);
    }

    this.validateWeights(); // Validate the total after distribution
  }

  // Validate weights and update error message if necessary
  private validateWeights(): void {
    const totalWeight = this.filterWeights.reduce((acc, pair) => acc + (pair.weight || 0), 0);

    if (totalWeight > 100) {
        this.weightError = 'Total weight cannot exceed 100%';
    } else if (totalWeight < 100 && this.filterWeights.every(pair => pair.weight !== null && pair.weight !== undefined)) {
        this.weightError = 'Total weight must be exactly 100%';
    } else {
        this.weightError = null; // Clear any error if weights are valid
    }
  }

}
