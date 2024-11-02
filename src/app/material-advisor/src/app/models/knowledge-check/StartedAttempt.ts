import { Attempt } from "./Attempt";
import { AttemptAnswer } from "./AttemptAnswer";

export interface StartedAttempt extends Attempt {
  submittedAnswers: AttemptAnswer[];
}