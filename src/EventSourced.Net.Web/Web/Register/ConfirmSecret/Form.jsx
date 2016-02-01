import { reduxForm } from 'redux-form'
import validate from './validation'
import ValidationSummary from '../../Shared/ValidationSummary'

const form = 'verify'
const fields = ['code', 'correlationId']
const config = { form, fields, validate, }

const Form = ({ fields: { code }, handleSubmit, submitting, submitFailed, error, errors, }) => (
  <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>
    <div className="form-group">
      <div className="col-md-3">
        <label className="control-label sr-only">Secret code</label>
        <input type="text" name="code" className="form-control" placeholder="Secret code" disabled={submitting} {...code} />
      </div>
    </div>
    <div className="form-group">
      <div className="col-md-10">
        <button type="submit" className="btn btn-default" disabled={submitting}>Verify</button>
      </div>
    </div>
    <ValidationSummary errors={{ error, ...errors }} visible={submitFailed && !submitting} />
  </form>
)

Form.propTypes = {
  fields: React.PropTypes.object.isRequired,
  handleSubmit: React.PropTypes.func.isRequired,
  submitting: React.PropTypes.bool.isRequired,
  formName: React.PropTypes.string.isRequired,
}

const select = () => ({ formName: form, })
export default reduxForm(config, select)(Form)
