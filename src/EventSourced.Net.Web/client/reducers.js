import { combineReducers } from 'redux'
import { reducer as formReducer} from 'redux-form'
import { routeReducer } from 'redux-simple-router'
import { handleActions } from 'redux-actions'
import { INITIALIZE_DONE } from './actions'
import { uiLogin, dataUserLogin } from './Users/Login/reducers'
import { uiRegister } from './Users/Register/reducers'

const dataServerInitialize = {
  [INITIALIZE_DONE]: {
    throw: (state, action) => Object.assign({}, state, {
      unavailable: true
    }),
    next: (state, action) => Object.assign({}, state,
      action.payload.state.server)
  }
}

const dataUserInitialize = {
  [INITIALIZE_DONE]: {
    next: (state, action) => Object.assign({}, state,
      action.payload.state.data.user)
  }
}

export default combineReducers({
  app: combineReducers({
    ui: combineReducers({
      login: handleActions(uiLogin, { }),
      register: handleActions(uiRegister, { })
    }),
    data: combineReducers({
      server: handleActions(dataServerInitialize, {
        initialized: false,
        unavailable: false
      }),
      user: handleActions({
        ...dataUserInitialize,
        ...dataUserLogin
      }, { })
    })
  }),
  form: formReducer,
  routing: routeReducer
})
