Once running, access the main Camunda 8 tools via your browser:

Web Modeler
http://localhost:6014

Model BPMN diagrams, DMN decisions, and forms directly in your browser.

Tasklist
http://localhost:6005/tasklist

Work on and complete human tasks assigned to you.

Operate
http://localhost:6004/operate

Monitor and troubleshoot running process instances.

Optimize
http://localhost:6007

Analyze and report on your processes with dashboards and insights.

Identity Service
http://localhost:6008

Central user and group management for all Camunda applications.

Keycloak (Admin Console)
http://localhost:6009

Authentication backend for managing realms, clients, and users.

🔑 Default Credentials

Keycloak Admin Console

URL: http://localhost:6009

User: admin

Password: admin

Identity Service

URL: http://localhost:6008

Use the Keycloak credentials to log in and manage users and groups.

Operate / Tasklist / Optimize / Web Modeler

Login with a user you create in Keycloak under the camunda-platform realm.

✅ First Login Workflow

Open Keycloak Admin Console → http://localhost:6009

Log in with admin / admin.

Create a new user inside the camunda-platform realm.

Set username and password.

Assign the user to a group/role as needed.

Use this new user to log into:

Operate: http://localhost:6004/operate

Tasklist: http://localhost:6005/tasklist

Optimize: http://localhost:6007

Web Modeler: http://localhost:6014

✅ Quick Health Check

Confirm that Zeebe (the workflow engine) is running on port 6001:

nc -zv localhost 6001


Check logs of a container (example for Operate):

docker logs operate

📖 Further Usage

For more advanced usage, configuration, and troubleshooting, see the official documentation:
👉 Camunda 8 Self-Managed Docker Compose


---

This is the **entire README.md** — nothing missing, no outside chatter.  

Want me to also add a **“Services Overview Table”** (service name → description → port → URL) so it’s even easier to scan?
