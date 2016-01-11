import { combineReducers } from 'redux'
import { reducer as formReducer} from 'redux-form'
import { routeReducer } from 'redux-simple-router'
import { RECEIVE_INITIAL_STATE, FAIL_INITIAL_STATE } from './actions'
import { SENT_LOGIN, FAILED_LOGIN, RECEIVED_LOGIN } from './Users/Login/actions'
import login from './Users/Login/reducers'
import { SENT_REGISTER, FAILED_REGISTER, RECEIVED_REGISTER } from './Users/Register/actions'
import register from './Users/Register/reducers'
import { camelize } from 'humps'

const initialState = {
  server: {
    initialized: false,
    unavailable: false
  },
  ui: {
    register: {},
    login: {}
  },
  data: {
    user: {
      username: undefined
    }
  }
}

export function createReducer(initialState, handlers) {
  return function reducer(state = initialState, action) {
    return handlers.hasOwnProperty(action.type)
      ? handlers[action.type](state, action)
      : state
  }
}

export function convertServerErrors(action) {
  const errors = {}, { serverErrors, formInput, messages } = action
  if (serverErrors) {
    for (let field in serverErrors) {
      if (!serverErrors.hasOwnProperty(field) || !serverErrors[field]) continue
      for (let serverError of serverErrors[field]) {
        if (errors[field]) break
        let reason = camelize(serverError.reason)
        let message = 'An unknown error occurred'
        if (messages[field] && messages[field][reason]) {
          message = formatMessage(messages[field][reason], formInput)
        }
        else if (serverError.message) {
          message = serverError.message
        }
        if (formInput[field]) {
          errors[field] = message
        }
        else if (!formInput._error) {
          errors._error = message
        }
      }
    }
  }
  return errors
}

function formatMessage(unformattedMessage, formInput) {
  let formattedMessage = '', startIndex, endIndex, token, tokenValue
  if (unformattedMessage.indexOf('{')) {
    for (let i = 0; i < unformattedMessage.length; i++) {
      if (unformattedMessage[i] === '{') {
        if (unformattedMessage[i+1] === '{') {
          formattedMessage += unformattedMessage[i] + unformattedMessage[i+1]
          ++i
          continue
        }
        startIndex = i
        endIndex = unformattedMessage.substr(startIndex).indexOf('}')
        token = unformattedMessage.substr(startIndex + 1, endIndex -1)
        if (formInput && formInput[token] && formInput[token]) {
          tokenValue = formInput[token].trim()
          formattedMessage += tokenValue
          i = startIndex + endIndex
          continue
        }
        else {
          formattedMessage += unformattedMessage[i]
        }
      }
      else {
        formattedMessage += unformattedMessage[i]
      }
    }
  }
  return formattedMessage
}

function app(state = initialState, action) {
  // app reducer
  let newState
  switch (action.type) {
    case RECEIVE_INITIAL_STATE:
      newState = Object.assign({}, state, action.state)
      return newState
    case FAIL_INITIAL_STATE:
      newState = Object.assign({}, state, {
        ...state,
        server: {
          ...state.server,
          unavailable: true
        }
      })
      return newState
    case SENT_LOGIN:
    case FAILED_LOGIN:
    case RECEIVED_LOGIN:
      newState = login(state, action)
      return newState
    case SENT_REGISTER:
    case FAILED_REGISTER:
    case RECEIVED_REGISTER:
      newState = register(state, action)
      return newState
    default:
      return state
  }
  return state
}

export default combineReducers({
  app,
  form: formReducer,
  routing: routeReducer
})
