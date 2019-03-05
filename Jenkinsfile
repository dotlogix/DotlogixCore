pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        echo 'Building started'
        git(url: 'https://git.dotlogixcloud.de/dotlogix/Core.git', branch: 'master', credentialsId: 'DotlogixGiteaApiToken')
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