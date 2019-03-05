pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        echo 'Building started'
		bat "\"C:\Program Files (x86)\Nuget\nuget.exe" restore Core.sln'
		bat "\"${tool 'MSBuild'}\" Core.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
        echo 'Building finished'
      }
    }
    stage('Test') {
      steps {
        echo 'Testing started'
        echo 'Testing finished'
      }
    }
    stage('Publish') {
      parallel {
        stage('Publish') {
          steps {
            echo 'Publishing started'
            echo 'Publishing finished'
          }
        }
        stage('Deploy') {
          steps {
            echo 'Deployment started'
            echo 'Deployment finished'
          }
        }
      }
    }
  }
}