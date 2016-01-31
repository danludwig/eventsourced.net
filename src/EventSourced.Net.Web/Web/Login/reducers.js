import { handleActions } from 'redux-actions'
import standardApi from '../Shared/standardApi'
import { INITIALIZE_STATE } from '../Shared/actions'
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
  [LOGIN.SENT]: standardApi.reducers.sent,
  [LOGIN.FAIL]: standardApi.reducers.fail,
  [LOGIN.DONE]: standardApi.reducers.done,
}, standardApi.initialState)
