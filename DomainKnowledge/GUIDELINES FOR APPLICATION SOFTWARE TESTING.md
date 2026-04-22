
### APPENDIX A: CHECKLIST ON UNIT TESTING
(This checklist to suggest areas for the definition of test cases is for information purpose only; and in no way is it meant to be an exhaustive list. Please also note that a negative tone that matches with Section 6.1 suggestions has been used)

**Input**
1. Validation rules of data fields do not match with the program/data specification.
2. Valid data fields are rejected.
3. Data fields of invalid class, range and format are accepted.
4. Invalid fields cause abnormal program end.

**Output**
1. Output messages are shown with misspelling, or incorrect meaning, or not uniform.
2. Output messages are shown while they are supposed not to be; or they are not shown while they are supposed to be.
3. Reports/Screens do not conform to the specified layout with misspelled data labels/titles, mismatched data label and information content, and/or incorrect data sizes.
4. Reports/Screens page numbering is out of sequence.
5. Reports/Screens breaks do not happen or happen at the wrong places.
6. Reports/Screens control totals do not tally with individual items.
7. Screen video attributes are not set/reset as they should be.

**File Access**
1. Data fields are not updated as input.
2. “No-file” cases cause program abnormal end.
3. “Empty-file” cases cause program abnormal end.
4. Program data storage areas do not match with the file layout.
5. The last input record (in a batch of transactions) is not updated.
6. The last record in a file is not read while it should be.
7. Deadlock occurs when the same record/file is accessed or updated by more than 1 user.

**Internal Logic**
1. Counters are not initialised as they should be.
2. Mathematical accuracy and rounding does not conform to the prescribed rules.

**Job Control Procedures**
1. A wrong program is invoked and/or the wrong library/files are referenced.
2. Program execution sequence does not follow the JCL condition codes or control scripts setting.
3. Run time parameters are not validated before use.

**Program Documentation**
1. Documentation is not consistent with the program behaviour.

**Program Structure (through program walkthrough)**
1. Coding structure does not follow installation standards.

**Performance**
1. The program runs longer than the specified response time.

**Sample Test Cases**
1. Screen labels checks.
2. Screen videos checks with test data set 1.
3. Creation of record with valid data set 2.
4. Rejection of record with invalid data set 3.
5. Error handling upon empty file 1.
6. Batch program run with test data set 4.

***

### APPENDIX B: CHECKLIST ON LINK/INTEGRATION TESTING
(This checklist to suggest areas for the definition of test cases is for information purpose only; and in no way is it meant to be an exhaustive list. Please also note that a negative tone that matches with Section 6.1 suggestions has been used)

**Global Data (e.g. Linkage Section)**
1. Global variables have different definition and/or attributes in the programs that referenced them.

**Program Interfaces**
1. The called programs are not invoked while they are supposed to be.
2. Any two interfaced programs have different number of parameters, and/or the attributes of these parameters are defined differently in the two programs.
3. Passing parameters are modified by the called program while they are not supposed to be.
4. Called programs behaved differently when the calling program calls twice with the same set of input data.
5. File pointers held in the calling program are destroyed after another program is called.

**Consistency among programs**
1. The same error is treated differently (e.g. with different messages, with different termination status etc.) in different programs.

**Sample Test Cases**
1. Interface test between programs xyz, abc & jkl.
2. Global (memory) data file 1 test with data set 1.

***

### APPENDIX C: CHECKLIST ON FUNCTION TESTING
(This checklist to suggest areas for the definition of test cases is for information purpose only; and in no way is it meant to be an exhaustive list. Please also note that a negative tone that matches with Section 6.1 suggestions has been used)

**Comprehensiveness**
1. Agreed business function is not implemented by any transaction/report.

**Correctness**
1. The developed transaction/report does not achieve the said business function.

**Sample Test cases**
1. Creation of records under user normal environment.
2. Enquiry of the same record from 2 terminals.
3. Printing of records when the printer is in normal condition.
4. Printing of records when the printer is off-line or paper out.
5. Unsolicited message sent to console/supervisory terminal when a certain time limit is reached.

***

### APPENDIX D: CHECKLIST ON SYSTEM TESTING
(This checklist to suggest areas for the definition of test cases is for information purpose only; and in no way is it meant to be an exhaustive list. Please also note that a negative tone that matches with Section 6.1 suggestions has been used)

**Volume Testing**
1. The system cannot handle a pre-defined number of transactions.

**Stress Testing**
1. The system cannot handle a pre-defined number of transactions over a short span of time.

**Performance Testing**
1. The response times are excessive over a pre-defined time limit under certain workloads.

**Recovery Testing**
1. Database cannot be recovered in event of system failure.
2. The system cannot be restarted after a system crash.

**Security Testing**
1. The system can be accessed by an unauthorised person.
2. The system does not log out automatically in event of a terminal failure.

**Procedure Testing**
1. The system is inconsistent with manual operation procedures.

**Regression Testing**
1. The sub-system / system being installed affect the normal operation of the other systems / sub-systems already installed.

**Operation Testing**
1. The information inside the operation manual is not clear and concise with the application system.
2. The operational manual does not cover all the operation procedures of the system.

**Sample Test Cases**
1. System performance test with workload mix 1.
2. Terminal is powered off when an update transaction is processed.
3. Security breakthrough - pressing different key combinations onto the logon screen.
4. Reload from backup tape.

好的，這是為您提取並按原文（英文）格式化輸出的**附錄 E 至附錄 H (Appendix E to H)** 的 Markdown 內容：

***

### APPENDIX E: CHECKLIST ON ACCEPTANCE TESTING
(This checklist to suggest areas for the definition of test cases is for information purpose only; and in no way is it meant to be an exhaustive list. Please also note that a negative tone that matches with Section 6.1 suggestions has been used)

**Comprehensiveness**
1. Agreed business function is not implemented by any transaction/report.

**Correctness**
1. The developed transaction/report does not achieve the said business function.

**Sample Test Cases**
(Similar to Function Testing)

***

### APPENDIX F: CHECKLIST FOR OUTSOURCED SOFTWARE DEVELOPMENT
1. Tailor and suitably incorporate the following in the tender specification or work assignment brief as appropriate.
2. Check for the inclusion of an overall test plan in the tender proposal or accept it as the first deliverable from the contractor.
3. Review and accept the different types of test plan, test specifications and test results.
4. Wherever possible, ask if there are any tools (ref. Section 10) to help demonstrate the completeness and test the coverage of the software developed.
5. Perform sample program walkthrough.
6. Ask for a periodic test progress report. (ref. Section 8.5)
7. Ask for the contractor’s contribution in preparing the Acceptance Test process.
8. Ask for a Test Summary Report (ref. Section 8.6) at the end of the project.
9. If third party independent testing service is required for a particular type of testing (e.g. system testing), define the scope of the testing service needed and state the requirements of the testing in a separate assignment brief or service specification for procurement of the independent testing services. Such service should be separately acquired from the procurement of software development. Besides, the service requirement for support and coordination with the Independent Testing Contractor should be added to the software development tender or work assignment brief.

***

### APPENDIX G: LIST OF SOFTWARE TESTING CERTIFICATIONS
1. **Software Certifications by the Quality Assurance Institute (QAI) Global Institute**
   QAI was established in 1980 in Orlando, Florida in U.S.A. It provides educational and training programs for development of IT professionals in different aspects including software quality assurance and testing. QAI issues the following certifications that qualify professional software testers and test managers:
   (i) Certified Associate in Software Testing (CAST)
   (ii) Certified Software Tester (CSTE)
   (iii) Certified Manager of Software Testing (CMST)

2. **International Software Testing Qualifications Board (ISTQB)**
   Founded in 2002, ISTQB is a not-for-profit association legally registered in Belgium. It aims to create a complete set of concepts on software testing which allows testers to get certification. It issues the following certifications:
   (i) Certified Tester Foundation Level
   (ii) Certified Tester Advanced Level – Test Manager
   (iii) Certified Tester Advanced Level – Test Analyst
   (iv) Certified Tester Advanced Level – Technical Test Analyst
   (v) Certified Tester Expert Level – Test Management

3. **BCS, The Chartered Institute for IT (formerly named as British Computer Society)**
   It was founded in 1957 aiming to promote the study and practice of information technology for examine the knowledge and skill about computer application according to the national computer rank system. It has provided the following software testing examinations:
   (i) 3rd Level – Software Testing Skill (軟件測試技術)
   (ii) 4th Level – Software Testing Engineer (軟件測試工程師)

***

### APPENDIX H: INDEPENDENT TESTING SERVICES

#### Independent Testing Services Activities
Independent testing services generally include the following testing activities:

**Perform test management on overall test process:**
(i) Manage test plan and test procedures;
(ii) Schedule and allocate resources to support testing services;
(iii) Organise regular meetings with project team and users to report the testing progress, discuss and resolve issues/problems, present test results and reports; and
(iv) Coordinate project team with users for test items and activities throughout the test process.

**1. Collect information and requirements for testing services:**
(i) Collect relevant information such as user requirements, security requirements, system specifications and architecture design to understand the system and user needs;
(ii) Conduct workshops / interviews with project team and users to identify and collect the detailed user requirements on testing services according to the service requirements specified in the assignment brief or service specification; and
(iii) Identify the test items i.e. which features are to be tested, and what are going to be validated.

**2. Define test strategy and create test plan:**
(i) Define the scope, goals and objectives of testing and test strategy based on requirements, project schedule, budget, resources, identified risks, etc.;
(ii) Define overall test process, approach to be taken and deliverables to be produced;
(iii) Estimate the test effort and resources required; and
(iv) Create a high-level master test plan and detailed test plans as necessary.

**3. Design test and prepare test specifications:**
(i) Define what needs to be tested in detail by examining the requirements or items to be tested, and identify any constraint for the test and the pass/fail/ending criteria for the test;
(ii) Design test procedures, test cases with different scenarios, test data covering all test scenarios and testing environment;
(iii) Prepare test specifications to document the details of test; and
(iv) Review and align with project team and/or users on test specifications especially for the test cases.

**4. Set up testing environment and conduct tests:**
(i) Set up the testing environment;
(ii) Schedule the tests;
(iii) Create sample test data and load test data to the testing environment;
(iv) Conduct tests according to the schedule and test specifications;
(v) Complete the tests, record the test results and create test incident reports if any; and
(vi) Re-run the tests on failed cases after rectification by project team until all cases have passed the testing.

**5. Document test results and prepare test reports:**
(i) Analyse test results, prepare test progress reports and test summary reports.

**6. Review test results and test reports:**
(i) Review test results and reports to ensure completeness of tests, covering all defined test cases; and
(ii) Ensure the validity of the test results.

#### Common Types of Independent Testing Services
**(a) Unit and Link/Integration Testing**
(i) Black-box / White-box Testing: Test the functionality of an application and application’s internal structure.
(ii) Code review: Check and verify the quality of program codes to ensure that the codes are written properly and securely.
(iii) Coverage Testing: Ensure the percentage of test coverage is high enough to reduce the likelihood of occurrence of bugs / errors containing in the program.

**(b) System Testing**
(i) Installation Testing: Ensure the system is installed and set up properly.
(ii) Operations Testing: Evaluate that the software application operates according to operational procedures.
(iii) Performance Testing: Conduct various tests such as stress test and load test to ensure that the system runs properly under the normal and abnormal loading, and meet the required performance level.
(iv) Regression Testing: Ensure that the original code is not affected by the changed code, and the system still works properly after the new changes are deployed.
(iv) Disaster Recovery Testing: Ensure that the data could be recovered in the disaster recovery site due to the hardware failure in production platform.
(v) Security Testing: Ensure that the security controls and measures imposed on the system can protect system data and programs against unauthorised access or security attacks by both internal and external parties.

**(c) Acceptance Testing**
(i) User Acceptance Testing: Develop user acceptance test plan, manage test process and conduct tests or assist users to conduct tests to ensure the end-to-end business processes are valid and capable to fulfil all business requirements.

#### Considerations for Selecting Parts of Testing to Outsourced Contractor
The project team should consider which parts of testing to outsource, including test levels, test types and test activities.

**(a) Test Level**
Programmers generally perform unit testing and link/integration testing by themselves during coding process. Therefore, it is sometimes not suitable for such testing to be outsourced to an external independent testing contractor especially if the development work is outsourced. This may lead to more time and effort for coordination and communication, and may affect the development progress.

User Acceptance Testing is required to be conducted in a production-like environment by users who are familiar with the business and know what they really want. Therefore, it is not suitable for the User Acceptance Testing to be conducted by external independent testing contractor who may not be familiar with the business and may not be capable to take up the user role to accept that the system satisfies all user requirements.

On the other hand, system testing is comparatively more suitable to be performed by an independent testing contractor. It is because system testing does not involve users and has less impact on development work. Besides, a separate testing environment can be set up and tests can be scheduled and conducted according to a well-defined test plan.

**(b) Test Type**
In general, both functional and non-functional system tests can be outsourced to an independent testing contractor. Among these, test types requiring regular repetition and test automation is more suitable for outsourcing in order to save costs. Examples are function test, regression test and compatibility test. Test types such as performance test, load test and ... *(Note: Text truncated in original source)*

#### Considerations for Selecting Testing Contractors
**(a) Experience in Software Testing**
Service provider that has experience in providing same or similar type of testing services to clients for project of related business and/or similar project nature is preferred.

**(b) Specialised Skill Set in Software Testing**
Service provider whose core business is software testing is more preferred as the supplier is more able to provide specialised and professional staff and resources.

**(c) Secured Environment for Protection of Test Deliverables**
... *(Note: Text truncated in original source)*

**(e) Well Design Infrastructure for Testing**
The technical infrastructure used by the service provider for software testing must be well designed to support comprehensive testing for the software products. It should consist of adequate hardware, software, network and other equipment such as PC, mobile devices, wireless devices, operation systems and different types of web browsers, etc.

**(f) Standard Documentation**
Service provider should have some testing guidelines and documentation established for use and control of the testing purpose.

**(g) Qualifications of Testers**
Staff of the service provider should preferably possess at least one valid and relevant qualified software testing certificate. Please refer to Appendix G for some examples of testing certifications issued by different organisations.

#### Roles and Responsibilities of Testing Contractors
The following shows an example of the roles and responsibilities of testers for independent test services.

| Roles | Responsibilities |
| :--- | :--- |
| **Test Manager / Test Specialist** | (i) Schedule and assign duties to subordinates <br> (ii) Plan and manage the test process <br> (iii) Resolve technical and non-technical issues and disputes related to the test process <br> (iv) Establish procedures and/or automated performance measurement capability to monitor the progress of testing <br> (v) Liaise with project team and developer’s team on day-to-day testing work <br> (vi) Develop project management plans and quality control parameters |
| **Test Coordinator** | (i) Schedule and assign testing tasks to team members <br> (ii) Assist in setting up the testing environment <br> (iii) Co-ordinate with all working parties in projects <br> (iv) Define the approach, methodology and tools used in testing <br> (v) Perform quality control and quality assurance in test process <br> (vi) Prepare test scenarios and produce documentation <br> (vii) Provide support and troubleshoot problems in test process. <br> (viii) Provide status of progress and defects to Test Manager <br> (ix) Assure conformance to standards and test procedures |
| **Test Lead** | (i) Supervise and lead the work of testing in the same team <br> (ii) Assign testing tasks to testers and assist in test execution when required <br> (iii) Produce and maintain testing status documentation |
| **Tester** | (i) Conduct testing according to test procedures <br> (ii) Produce and maintain test documentation |