import { combineReducers } from 'redux'
import challengeContact from './ChallengeContact/reducers'
import confirmSecret from './ConfirmSecret/reducers'
import createLogin from './CreateLogin/reducers'
import validateUsername from './ValidateUsername/reducers'

export default combineReducers({
  challengeContact,
  confirmSecret,
  createLogin,
  validateUsername,
})
