import { handleActions } from 'redux-actions'
import standardApi from '../Shared/standardApi'
import { LOGOFF } from '../Logoff/actions'

export const logoff = handleActions({
  [LOGOFF.SENT]: standardApi.reducers.sent,
  [LOGOFF.FAIL]: standardApi.reducers.fail,
  [LOGOFF.DONE]: standardApi.reducers.done,
}, standardApi.initialState)
