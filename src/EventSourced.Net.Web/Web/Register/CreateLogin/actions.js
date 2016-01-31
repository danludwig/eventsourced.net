import standardApi from '../../Shared/standardApi'
import { createAction } from 'redux-actions'
import { routeActions } from 'redux-simple-router'
import selectCommandRejectionErrors from '../../Shared/selectors/commandRejectionErrors'
import selectSocketReversalErrors from '../../Shared/selectors/socketReversalErrors'
import { messages } from './validation'
import _ from 'lodash'

export const REDEEM = {
  SENT: 'REGISTER/REDEEM_SENT',
  FAIL: 'REGISTER/REDEEM_FAIL',
  DONE: 'REGISTER/REDEEM_DONE',
  OVER: 'REGISTER/REDEEM_OVER',
}

export default (formInput, dispatch) => {
  return new Promise((resolve, reject) => {

    const { correlationId, token, ...body } = formInput
    return dispatch(standardApi.createAction({
      types: [REDEEM.SENT, REDEEM.DONE, REDEEM.FAIL],
      method: 'POST',
      endpoint: `/api/register/${correlationId}/redeem?token=${encodeURIComponent(token)}`,
      body: body,
    }))
    .then(action => {
      const errors = selectCommandRejectionErrors(body, action, messages)
      if (errors) return reject(errors)

      const location = action.payload.headers.location
      const socketUrl = action.payload.headers.correlationSocket
      const socket = new WebSocket(socketUrl)
      socket.onmessage = socketMessage => {
        var messageData = { jsonParseFailed: true }
        try { messageData = JSON.parse(socketMessage.data) } catch (ex) { }
        return dispatch(createAction(REDEEM.OVER)(messageData))
          .then(socketAction => {
            const socketErrors = selectSocketReversalErrors(body, socketAction, messages)
            return socketErrors ?
              reject(socketErrors) :
              dispatch(routeActions.push(location))
          })
      }
    })
  })
}
