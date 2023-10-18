# todo items: 
- add more interesting plugins
- when starting recording, start a timer
- when receiving new recordings for transcription when either timer or accumulated text hits a certain threshold, have sk summarize the text down and continue recording/appending text
		- consider overlapping chunks (how to do for time?) or is this not necessary due to summarization? do we include previous summaries in the next summary? would that be too much semantic reduction potentially?
	- potential alternative: generate plans based on chunks?
- when user stops recording, stop timer and generate plan
- offer the plan to the user and allow them to remove actions from the plan or rearrange the order? is this possible?
- potentially decide on plan type by the summary of user input => should it be action/sequential/stepwise planner?
- give user control on plan execution

# disparate thoughts
whisper to text => text analytics for health => UI shows doc that suggests medication request with filled out order for user to confirm
copilot use: did you think about x? here is how you can do x! or... I've prepared x for you to do!

25 min on UI/experience
- where does it falter (e.g. generating fhir) vs where does it shine (e.g. extract semantic meaning)
- translate user natural language to actionable items
- THOUGHT: capture what user says that planner can't find satisfying plugin for to capture future enhancement ideas
25 min on how it works
10 min QnA

incorporate memories? incorporate commander/writer/safeguard strategy?