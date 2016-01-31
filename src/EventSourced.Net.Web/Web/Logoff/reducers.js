import { handleActions } from 'redux-actions'
import standardApi from '../Shared/reducers/standardApi'
import { LOGOFF } from '../Logoff/actions'

export const logoff = handleActions({
  [LOGOFF.SENT]: standardApi.sent,
  [LOGOFF.FAIL]: standardApi.fail,
  [LOGOFF.DONE]: standardApi.done,
}, standardApi.initialState)
