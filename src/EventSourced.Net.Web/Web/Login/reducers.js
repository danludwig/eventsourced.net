import { handleActions } from 'redux-actions'
import standardApi from '../Shared/standardApi'
import { LOGIN, LOGOFF } from './actions'
import { INITIALIZE_STATE } from '../Shared/actions'

const initialState = { apiCalls: [], }

export const login = handleActions({
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
}, initialState)

export const logoff = handleActions({
  [LOGOFF.SENT]: standardApi.reducers.sent,
  [LOGOFF.FAIL]: standardApi.reducers.fail,
  [LOGOFF.DONE]: standardApi.reducers.done,
}, initialState)
