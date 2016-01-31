import { handleActions } from 'redux-actions'
import standardApi from '../Shared/reducers/standardApi'
import { INITIALIZE_STATE } from '../Shared/actions/initializeState'
import { LOGIN } from './actions'
import { LOGOFF } from '../Logoff/actions'

const initialState = { apiCalls: [], }

export default handleActions({
  [INITIALIZE_STATE]: (state, action) =>
    Object.assign({}, state, {
      username: action.payload.app.login.username,
    }
  ),
  [LOGIN.DATA]: (state, action) =>
    Object.assign({}, state, {
      username: action.payload.username,
    }
  ),
  [LOGOFF.DATA]: (state, action) =>
    Object.assign({}, state, {
      username: undefined,
    }
  ),
  [LOGIN.SENT]: standardApi.sent,
  [LOGIN.FAIL]: standardApi.fail,
  [LOGIN.DONE]: standardApi.done,
}, standardApi.initialState)
