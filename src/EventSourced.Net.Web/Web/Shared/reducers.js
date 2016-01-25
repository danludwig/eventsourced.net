import { combineReducers } from 'redux'
import { reducer as formReducer} from 'redux-form'
import { routeReducer } from 'redux-simple-router'
import { handleActions } from 'redux-actions'
import { INITIALIZE_STATE } from './actions'
import { uiLogin, dataUserLogin } from '../Users/Login/reducers'
import { uiRegister, uiVerify, uiRedeem } from '../Users/Register/reducers'
import { handlers as uiCheckUsername } from '../Users/Register/CheckUsernameField'

const dataUserInitialize = {
  [INITIALIZE_STATE]: (state, action) =>
    Object.assign({}, state,
      action.payload.app.data.user),
}

export default combineReducers({
  app: combineReducers({
    ui: combineReducers({
      login: uiLogin,
      register: uiRegister,
      verify: uiVerify,
      redeem: uiRedeem,
      checkUsername: uiCheckUsername
    }),
    data: combineReducers({
      server: handleActions({}, {
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
